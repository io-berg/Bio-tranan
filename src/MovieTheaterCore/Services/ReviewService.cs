using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;

namespace MovieTheaterCore.Services
{
    public class ReviewService
    {
        IRepository<Review> _reviewRepository;
        MovieViewingService _movieViewingService;
        MovieService _movieService;

        public ReviewService(IRepository<Review> reviewRepository, MovieService movieService, MovieViewingService movieViewingService)
        {
            _reviewRepository = reviewRepository;
            _movieService = movieService;
            _movieViewingService = movieViewingService;
        }

        public async Task<IEnumerable<Review>> GetAllByMovieId(long id)
        {
            return await _reviewRepository.ListAsync(r => r.MovieId == id);
        }

        public async Task<Review> GetByIdAsync(long id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);

            return review;
        }

        public async Task<Review> AddReviewAsync(ReviewCreationModel review)
        {
            if (String.IsNullOrWhiteSpace(review.ReviewerName)) review.ReviewerName = "Anonymous";
            Review newReview = new Review()
            {
                MovieId = review.MovieId,
                ReviewerName = review.ReviewerName,
                Score = review.Score,
                ReviewContent = review.ReviewContent
            };

            await _reviewRepository.InsertAsync(newReview);
            return newReview;
        }

        public bool IsEligableForReview(MovieViewing movieViewing)
        {
            if (movieViewing == null) return false;
            return movieViewing.ViewingStart < DateTime.Now;
        }
    }
}