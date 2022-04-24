using MovieTheaterCore.Services;

namespace WebApp.Services;

public class CreateReviewViewModelService
{
    ReservationViewModelService _reservationViewModelService;
    ReservationService _reservationService;
    ReviewService _reviewService;

    public CreateReviewViewModelService(ReservationViewModelService reservationViewModelService, ReservationService reservationService, ReviewService reviewService)
    {
        _reservationViewModelService = reservationViewModelService;
        _reservationService = reservationService;
        _reviewService = reviewService;
    }

    public async Task<CreateReviewViewModel> GetModel(string reservationCode)
    {
        CreateReviewViewModel model = new();
        var reservation = await _reservationService.GetByCode(reservationCode);
        if (reservation == null) return null;

        if (!_reviewService.IsEligableForReview(reservation.MovieViewing)) return null;

        var reservationModel = await _reservationViewModelService.GetModelByCode(reservationCode);
        if (reservationModel == null) return null;

        model.MovieId = reservationModel.Viewing.Movie.Id;
        model.ReservationCode = reservationModel.ReservationCode;
        model.MovieTitle = reservationModel.Viewing.Movie.Title;

        return model;
    }
}