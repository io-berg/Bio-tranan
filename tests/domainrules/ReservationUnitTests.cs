using System;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using Xunit;

namespace domainrules;

public class ReservationUnitTests : IDisposable
{
    private MovieTheaterContext _context;
    private MovieViewingService _viewingService;
    private MovieService _movieService;
    private ReservationService _reservationService;
    private readonly IRepository<MovieViewing> _viewingRepo;
    private readonly IRepository<Movie> _movieRepo;
    private readonly IRepository<Salon> _salonRepo;
    private readonly IRepository<Actor> _actorRepo;
    private readonly IRepository<Director> _directorRepo;
    private readonly IRepository<Reservation> _reservationRepo;

    public ReservationUnitTests()
    {
        var options = new DbContextOptionsBuilder<MovieTheaterContext>()
            .UseInMemoryDatabase(databaseName: "MovieTheater")
            .Options;

        _context = new MovieTheaterContext(options);
        _actorRepo = new Repository<Actor>(_context);
        _directorRepo = new Repository<Director>(_context);
        _movieRepo = new Repository<Movie>(_context);
        _salonRepo = new Repository<Salon>(_context);
        _reservationRepo = new Repository<Reservation>(_context);
        _viewingRepo = new Repository<MovieViewing>(_context);
        _movieService = new MovieService(_movieRepo, _actorRepo, _directorRepo);
        _viewingService = new MovieViewingService(_viewingRepo, _salonRepo, _movieService);
        _reservationService = new ReservationService(_reservationRepo, _viewingService);

        _salonRepo.InsertAsync(new Salon() { Name = "Salon 1", Seats = 10 }).Wait();
        _movieRepo.InsertAsync(new Movie() { Title = "Movie 1", Genre = "Genre 1", Description = "Description 1", SpokenLanguage = "Spoken Language 1", TextLanguage = "Text Language 1", Runtime = 100, ReleaseYear = 2000, AgeRating = 18, AllowedViewings = 2 }).Wait();
        _viewingRepo.InsertAsync(new MovieViewing() { MovieId = 1, SalonId = 1, ViewingStart = new DateTime(2024, 1, 1, 2, 2, 2), TicketPrice = 10 }).Wait();
    }

    [Fact]
    public async Task TestThatYouCantBookMoreSeatsThanAvailableAsync()
    {
        // Arrange
        bool expected = false;
        bool result = true;
        var reservation = new ReservationCreationModel
        {
            MovieViewingId = 1,
            Seats = 11
        };

        // Act
        try
        {
            await _reservationService.AddReservationAsync(reservation);
        }
        catch (Exception)
        {
            result = false;
        }

        // Assert

        Assert.Equal(expected, result);
    }



    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}