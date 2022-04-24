using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using WebApp.Models;
using WebApp.Models.ApiModels;

namespace WebApp.Services;

public class MovieViewingViewModelService
{
    private readonly IRepository<MovieViewing> _movieViewingRepository;
    private readonly IRepository<Reservation> _reservationRepository;
    private MovieViewModelService _movieService;
    private SalonViewModelService _salonService;
    private IConfiguration _appsettings;
    private HttpClient _client;

    public MovieViewingViewModelService(IRepository<MovieViewing> movieViewingRepository,
                                        IRepository<Reservation> reservationRepository,
                                        MovieViewModelService movieViewModelService,
                                        SalonViewModelService salonViewModelService,
                                        IConfiguration appsettings,
                                        HttpClient client)
    {
        _movieViewingRepository = movieViewingRepository;
        _movieService = movieViewModelService;
        _salonService = salonViewModelService;
        _reservationRepository = reservationRepository;
        _appsettings = appsettings;
        _client = client;
    }

    public async Task<MovieViewingViewModel> GetViewingModelsAsync(long id)
    {
        var viewing = await _movieViewingRepository.GetByIdAsync(id);

        if (viewing == null)
        {
            return null;
        }

        MovieViewingViewModel model = new();
        model.Id = viewing.Id;
        model.Movie = await _movieService.GetModelByIdAsync(viewing.MovieId);
        model.Salon = await _salonService.GetModelById(viewing.SalonId);
        model.ViewingStart = viewing.ViewingStart;
        model.TicketPrice = viewing.TicketPrice;
        model.ReservedSeats = _reservationRepository.ListAsync(r => r.MovieViewingId == viewing.Id).Result.Sum(r => r.Seats);
        model.imgLink = await GetImageLink(model.Movie.Title);

        return model;
    }

    public async Task<MovieViewingViewModelSmall> GetSmallViewingModelsAsync(long id)
    {
        var viewing = await _movieViewingRepository.GetByIdAsync(id);

        if (viewing == null)
        {
            return null;
        }

        MovieViewingViewModelSmall model = new();
        model.Id = viewing.Id;
        model.Movie = await _movieService.GetSmallModelByIdAsync(viewing.MovieId);
        model.Salon = await _salonService.GetModelById(viewing.SalonId);
        model.ViewingStart = viewing.ViewingStart;
        model.TicketPrice = viewing.TicketPrice;
        model.ReservedSeats = _reservationRepository.ListAsync(r => r.MovieViewingId == viewing.Id).Result.Sum(r => r.Seats);

        return model;
    }

    public async Task<List<MovieViewingViewModelSmall>> GetUpcomingViewingModelsAsync()
    {
        var viewings = await _movieViewingRepository.ListAsync(mv => mv.ViewingStart > DateTime.Now);

        if (viewings == null)
        {
            return new List<MovieViewingViewModelSmall>();
        }

        List<MovieViewingViewModelSmall> model = new();
        model = viewings.Select(v => new MovieViewingViewModelSmall
        {
            Id = v.Id,
            Movie = _movieService.GetSmallModelByIdAsync(v.MovieId).Result,
            Salon = _salonService.GetModelById(v.SalonId).Result,
            ViewingStart = v.ViewingStart,
            TicketPrice = v.TicketPrice,
            ReservedSeats = _reservationRepository.ListAsync(r => r.MovieViewingId == v.Id).Result.Sum(r => r.Seats)
        }).ToList();

        return model;
    }

    private async Task<string> GetImageLink(string movieTitle)
    {
        string apiKey = _appsettings.GetSection("SuperSecretApiKeys").GetSection("MovieApi").Value;
        var response = await _client.GetAsync($"https://imdb-api.com/en/API/SearchMovie/{apiKey}/{movieTitle}");
        var content = await response.Content.ReadFromJsonAsync<ApiRoot>();
        if (String.IsNullOrWhiteSpace(content.errorMessage))
        {
            return content.Results[0].image;
        }
        return null;
    }
}