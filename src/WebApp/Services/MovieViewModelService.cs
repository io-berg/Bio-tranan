using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;
using MovieTheaterCore.Services;
using WebApp.Models;

namespace WebApp.Services
{
    public class MovieViewModelService
    {
        private readonly MovieService _movieService;

        public MovieViewModelService(MovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<MovieViewModel> GetModelByIdAsync(long id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            var model = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Directors = movie.Directors.Select(d => d.Name).ToList(),
                Actors = movie.Actors.Select(a => a.Name).ToList(),
                Description = movie.Description,
                SpokenLanguage = movie.SpokenLanguage,
                TextLanguage = movie.TextLanguage,
                Runtime = movie.Runtime,
                ReleaseYear = movie.ReleaseYear,
                AgeRating = movie.AgeRating
            };

            return model;
        }

        public async Task<MovieViewModelSmall> GetSmallModelByIdAsync(long id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            var model = new MovieViewModelSmall
            {
                Id = movie.Id,
                Title = movie.Title,
                SpokenLanguage = movie.SpokenLanguage,
                TextLanguage = movie.TextLanguage,
                Runtime = movie.Runtime,
                ReleaseYear = movie.ReleaseYear,
                AgeRating = movie.AgeRating
            };

            return model;
        }
    }
}