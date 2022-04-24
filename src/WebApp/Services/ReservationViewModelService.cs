using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public class ReservationViewModelService
    {
        IRepository<Reservation> _repository;
        MovieViewingViewModelService _movieViewingViewModelService;

        public ReservationViewModelService(IRepository<Reservation> repository, MovieViewingViewModelService movieViewingViewModelService)
        {
            _repository = repository;
            _movieViewingViewModelService = movieViewingViewModelService;
        }

        public async Task<ReservationViewModel> GetModelByCode(string code)
        {
            var reservation = _repository.ListAsync(r => r.ReservationCode == code).Result.FirstOrDefault();
            if (reservation == null) return null;

            var model = new ReservationViewModel();
            model.Viewing = await _movieViewingViewModelService.GetSmallViewingModelsAsync(reservation.MovieViewingId);
            model.Seats = reservation.Seats;
            model.ReservationCode = reservation.ReservationCode;
            model.TotalPrice = reservation.Seats * model.Viewing.TicketPrice;

            return model;
        }
    }
}