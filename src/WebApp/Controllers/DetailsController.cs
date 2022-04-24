using Microsoft.AspNetCore.Mvc;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class DetailsController : Controller
    {
        private MovieViewingViewModelService _movieViewingViewModelService;
        private ReviewService _reviewService;

        public DetailsController(MovieViewingViewModelService movieViewingViewModelService, ReviewService reviewService)
        {
            _movieViewingViewModelService = movieViewingViewModelService;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index(long id)
        {
            DetailsViewModel model = new();
            model.Viewing = await _movieViewingViewModelService.GetViewingModelsAsync(id);
            model.Reviews = await _reviewService.GetAllByMovieId(model.Viewing.Movie.Id);

            if (model.Viewing == null) return NotFound();

            return View(model);
        }
    }
}