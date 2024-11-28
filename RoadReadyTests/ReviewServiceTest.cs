using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoadReady.Controllers;
using RoadReady.Data;
using RoadReady.Models;
using RoadReady.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadReadyTests
{
    internal class ReviewServiceTest
    {
        private Mock<IReviewService> _mockReviewService;
        private ReviewController _controller;
        private Mock<ILogger<ReviewController>> _mockLogger;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _mockReviewService = new Mock<IReviewService>();
            _mockLogger = new Mock<ILogger<ReviewController>>();
            _controller = new ReviewController(_context,_mockReviewService.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAllReviews_ReturnsOkResult_WithListOfReviews()
        {
            // Arrange
            var reviews = new List<Review> {
                new Review { ReviewId = 1, Rating = 8, Comments = "Excellent Car Service." },
                new Review { ReviewId = 2, Rating = 9, Comments = "Car was Very maintained and excellent in use." }
            };
            _mockReviewService.Setup(service => service.GetAllReviews()).Returns(reviews);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(reviews, okResult?.Value);
        }

        [Test]
        public void GetReviewById_ReviewExists_ReturnsOkResult_WithReview()
        {
            // Arrange
            var reviewId = 1;
            var review = new Review { ReviewId = reviewId, Rating = 9, Comments = "Car was Very maintained and excellent in use." };
            _mockReviewService.Setup(service => service.GetReviewById(reviewId)).Returns(review);

            // Act
            var result = _controller.GetReviewById(reviewId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(review, okResult?.Value);
        }

        [Test]
        public void AddReview_ValidReview_ReturnsCreatedAtAction()
        {
            // Arrange
            var newReview = new Review {Rating = 9, Comments = "Car was Very maintained and excellent in use." };
            _mockReviewService.Setup(service => service.AddReview(newReview)).Returns(newReview.ReviewId);

            // Act
            var result = _controller.Post(newReview);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(newReview.ReviewId, createdResult?.RouteValues["id"]);
            Assert.AreEqual(newReview, createdResult?.Value);
        }

        [Test]
        public void UpdateReview_ValidIdAndReview_ReturnsOkResult()
        {
            // Arrange
            var reviewId = 1;
            var updatedReview = new Review { ReviewId = reviewId, Rating = 9, Comments = "Excellent Car Service" };
            _mockReviewService.Setup(service => service.UpdateReview(updatedReview)).Returns("Review updated successfully");

            // Act
            var result = _controller.Put(updatedReview);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Review updated successfully", okResult?.Value?.ToString());
        }

        [Test]
        public void DeleteReview_ValidId_ReturnsOkResult()
        {
            // Arrange
            var reviewId = 1;
            _mockReviewService.Setup(service => service.DeleteReview(reviewId)).Returns("Review deleted successfully");

            // Act
            var result = _controller.Delete(reviewId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Review deleted successfully", okResult?.Value?.ToString());
        }
    }
}
