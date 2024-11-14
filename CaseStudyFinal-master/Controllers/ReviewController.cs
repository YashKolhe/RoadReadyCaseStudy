using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound("Review not found");

            return Ok(review);
        }

        [HttpPost]
        public IActionResult Post(Review review)
        {
            var result = _reviewService.AddReview(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = result }, review);
        }

        [HttpPut]
        public IActionResult Put(Review review)
        {
            var result = _reviewService.UpdateReview(review);
            if (result == "Review not found")
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _reviewService.DeleteReview(id);
            if (result == "Review not found")
                return NotFound(result);

            return Ok(result);
        }
    }
}
