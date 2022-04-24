using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;

namespace MovieTheaterCore.Services
{
    public class MovieService
    {
        IRepository<Movie> _movieRepository;
        IRepository<Actor> _actorRepository;
        IRepository<Director> _directorRepository;

        public MovieService(IRepository<Movie> repository, IRepository<Actor> actorRepository, IRepository<Director> directorRepository)
        {
            _movieRepository = repository;
            _actorRepository = actorRepository;
            _directorRepository = directorRepository;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _movieRepository.InsertAsync(movie);
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            var list = await _movieRepository.ListAsync();
            if (list == null) return null;

            foreach (var movie in list)
            {
                movie.Actors = await PopulateActors(movie);
                movie.Directors = await PopulateDirectors(movie);
            }

            return (List<Movie>)list;
        }

        public async Task<Movie> GetByIdAsync(long id)
        {
            Movie movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return null;

            movie.Actors = await PopulateActors(movie);
            movie.Directors = await PopulateDirectors(movie);

            return movie;
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            await _movieRepository.DeleteAsync(movie);
        }

        private async Task<List<Director>> PopulateDirectors(Movie movie)
        {
            return (List<Director>)await _directorRepository.ListAsync(d => d.MovieId == movie.Id);
        }

        public async Task<List<Actor>> PopulateActors(Movie movie)
        {
            return (List<Actor>)await _actorRepository.ListAsync(a => a.MovieId == movie.Id);
        }
    }
}