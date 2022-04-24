using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ReviewController : Controller
    {
        ReviewService _reviewService;
        ReservationService _reservationService;
        CreateReviewViewModelService _createReviewViewModelService;

        public ReviewController(ReviewService reviewService, ReservationService reservationService, CreateReviewViewModelService createReviewViewModelService)
        {
            _reviewService = reviewService;
            _reservationService = reservationService;
            _createReviewViewModelService = createReviewViewModelService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create(string reservationCode)
        {
            var model = await _createReviewViewModelService.GetModel(reservationCode);
            if (model == null) return NotFound("Sorry you can't review this movie yet");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ReviewCreationModel review)
        {
            Review createdReview;
            try
            {
                createdReview = await _reviewService.AddReviewAsync(review);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (createdReview == null) return BadRequest("Review could not be created");

            var reservation = await _reservationService.GetByCode(review.ReservationCode);
            await _reservationService.DeleteReservationAsync(reservation);

            return View();
        }
    }
}