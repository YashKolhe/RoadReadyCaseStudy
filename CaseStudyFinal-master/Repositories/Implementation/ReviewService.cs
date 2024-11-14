using RoadReady.Data;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReviewById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public int AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review.ReviewId;
        }

        public string UpdateReview(Review review)
        {
            var existingReview = _context.Reviews.Find(review.ReviewId);
            if (existingReview == null) return "Review not found";

            existingReview.Rating = review.Rating;
            existingReview.Comments = review.Comments;

            _context.SaveChanges();
            return "Review updated successfully";
        }

        public string DeleteReview(int id)
        {
            var existingReview = _context.Reviews.Find(id);
            if (existingReview == null) return "Review not found";

            _context.Reviews.Remove(existingReview);
            _context.SaveChanges();
            return "Review deleted successfully";
        }
    }
}
