using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;

namespace MovieTheaterCore.Services
{
    public class MovieViewingService
    {
        IRepository<MovieViewing> _movieViewingRepository;
        IRepository<Salon> _salonRepository;
        MovieService _movieService;

        public MovieViewingService(IRepository<MovieViewing> movieViewingRepository, IRepository<Salon> salonRepository, MovieService movieService)
        {
            _movieViewingRepository = movieViewingRepository;
            _salonRepository = salonRepository;
            _movieService = movieService;
        }

        public async Task<MovieViewing> AddMovieViewingAsync(MovieViewingCreationModel CreationModel)
        {
            MovieViewing movieViewing = new MovieViewing();

            movieViewing.TicketPrice = CreationModel.TicketPrice;
            movieViewing.ViewingStart = CreationModel.ViewingStart;
            movieViewing.Movie = await _movieService.GetByIdAsync(CreationModel.MovieId);
            movieViewing.Salon = await _salonRepository.GetByIdAsync(CreationModel.SalonId);

            if (!await IsValidViewingAsync(movieViewing)) throw (new Exception("Invalid Movie Viewing"));

            await _movieViewingRepository.InsertAsync(movieViewing);

            return movieViewing;
        }

        public async Task<List<MovieViewing>> GetAllAsync()
        {
            var list = await _movieViewingRepository.ListAsync();

            foreach (var movieViewing in list)
            {
                movieViewing.Movie = await _movieService.GetByIdAsync(movieViewing.MovieId);
                movieViewing.Salon = await _salonRepository.GetByIdAsync(movieViewing.SalonId);
            }

            return (List<MovieViewing>)list;
        }

        public async Task<MovieViewing> GetByIdAsync(long id)
        {
            MovieViewing movieViewing = await _movieViewingRepository.GetByIdAsync(id);

            if (movieViewing != null)
            {
                movieViewing.Movie = await _movieService.GetByIdAsync(movieViewing.MovieId);
                movieViewing.Salon = await _salonRepository.GetByIdAsync(movieViewing.SalonId);
            }

            return movieViewing;
        }

        private async Task<bool> IsValidViewingAsync(MovieViewing movieViewing)
        {
            if (movieViewing.ViewingStart < DateTime.Now) return false;
            if (movieViewing.Movie == null) return false;
            if (movieViewing.Salon == null) return false;
            if (await HasReachedMaxViewings(movieViewing.Movie)) throw (new Exception("Movie has reached max viewings"));
            if (!IsFreeTimeslot(movieViewing)) throw (new Exception("Schedule is not free at this time"));

            return true;
        }

        private async Task<Boolean> HasReachedMaxViewings(Movie movie)
        {
            var matches = await _movieViewingRepository.ListAsync(mv => mv.MovieId == movie.Id);
            if (matches.Count() >= movie.AllowedViewings) return true;
            return false;
        }

        private bool IsFreeTimeslot(MovieViewing movieViewing)
        {
            // Get all viewings in the salon on the date of the viewing
            var matchingDay = _movieViewingRepository.ListAsync(mv => mv.ViewingStart.Date == movieViewing.ViewingStart.Date && mv.Salon.Id == movieViewing.Salon.Id).Result;
            if (matchingDay.Count() > 0)
            {
                foreach (var viewing in matchingDay)
                {
                    viewing.Movie = _movieService.GetByIdAsync(viewing.MovieId).Result;
                }

                if (ConflictsWithScedule(movieViewing, (List<MovieViewing>)matchingDay)) return false;
            }

            return true;
        }

        private bool ConflictsWithScedule(MovieViewing viewing, List<MovieViewing> scedule)
        {
            var movieEnds = viewing.ViewingStart.AddMinutes(viewing.Movie.Runtime);
            if (scedule.Any(mv => mv.ViewingStart > viewing.ViewingStart && mv.ViewingStart <= movieEnds)) return true;
            if (scedule.Any(mv => mv.ViewingStart <= viewing.ViewingStart && mv.ViewingStart.AddMinutes(mv.Movie.Runtime) > viewing.ViewingStart)) return true;

            return false;
        }
    }
}