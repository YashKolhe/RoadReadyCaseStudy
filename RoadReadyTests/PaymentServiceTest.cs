using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    internal class PaymentServiceTest
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<DbSet<Payment>> _mockPaymentsDbSet;
        private Mock<IPaymentService> _mockPaymentService;
        private PaymentController _controller;
        private Mock<ILogger<PaymentController>> _mockLogger;
        private ApplicationDbContext _context;
        private Mock<DbSet<User>> _mockUsersDbSet;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();

            // Mock DbSet for Users and Payments
            _mockUsersDbSet = new Mock<DbSet<User>>();
            _mockPaymentsDbSet = new Mock<DbSet<Payment>>();

            // Set up the mock context to return the mock DbSet
            _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);
            _mockContext.Setup(c => c.Payments).Returns(_mockPaymentsDbSet.Object);

            _mockPaymentService = new Mock<IPaymentService>();
            _mockLogger = new Mock<ILogger<PaymentController>>();
            _controller = new PaymentController(_context,_mockPaymentService.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAlPaymentrs_ReturnsOkResult_WithListOfPayments()
        {
            // Arrange
            var Payments = new List<Payment> {
                new Payment { PaymentId = 1, Amount = 8999, Status = "Pending" },
                new Payment { PaymentId = 2, Amount = 7999, Status = "Completed" }
            };
            _mockPaymentService.Setup(service => service.GetAllPayments()).Returns(Payments);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(Payments, okResult?.Value);
        }

        [Test]
        public void GetPaymentById_PaymentExists_ReturnsOkResult_WithPayment()
        {
            // Arrange
            var paymentId = 1;
            var Payment = new Payment { PaymentId = paymentId, Amount = 8999, Status = "Pending" };
            _mockPaymentService.Setup(service => service.GetPaymentById(paymentId)).Returns(Payment);

            // Act
            var result = _controller.GetPaymentById(paymentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(Payment, okResult?.Value);
        }

        
        [Test]
        public void AddPayment_ValidPayment_ReturnsCreatedAtAction()
        {
            // Arrange
            var newPayment = new Payment { Amount = 8999, Status = "Pending" };
            _mockPaymentService.Setup(service => service.AddPayment(newPayment)).Returns(newPayment.PaymentId);

            // Act
            var result = _controller.Post(newPayment);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(newPayment.PaymentId, createdResult?.RouteValues["id"]);
            Assert.AreEqual(newPayment, createdResult?.Value);
        }

        [Test]
        public void UpdatePayment_ValidIdAndPayment_ReturnsOkResult()
        {
            // Arrange
            var paymentId = 1;
            var updatedPayment = new Payment { PaymentId = paymentId, Amount = 8999, Status = "Pending" };
            _mockPaymentService.Setup(service => service.UpdatePayment(updatedPayment)).Returns("Payment updated successfully");

            // Act
            var result = _controller.Put(updatedPayment);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Payment updated successfully", okResult?.Value?.ToString());
        }

        [Test]
        public void DeletePayment_ValidId_ReturnsOkResult()
        {
            // Arrange
            var paymentId = 1;
            _mockPaymentService.Setup(service => service.DeletePayment(paymentId)).Returns("Payment deleted successfully");

            // Act
            var result = _controller.Delete(paymentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Payment deleted successfully", okResult?.Value?.ToString());
        }
        //[Test]
        //public async Task GetPaymentsByUserId_UserExistsWithPayments_ReturnsOkResult_WithPayments()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var user = new User { UserId = userId, FirstName = "Chris", Email = "chris@example.com", Role = "User" };
        //    var payments = new List<Payment>
        //    {
        //        new Payment { PaymentId = 1, UserId = userId, Amount = 10000, Status = "Pending" },
        //        new Payment { PaymentId = 2, UserId = userId, Amount = 5000, Status = "Completed" }
        //    };

        //    // Mock the users DbSet to return the user when searched
        //    _mockUsersDbSet.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);

        //    // Mock the payments DbSet to return the payments list
        //    _mockPaymentsDbSet.Setup(m => m.Where(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>())).Returns(payments.AsQueryable());

        //    // Act
        //    var result = await _controller.GetPaymentsByUserId(userId);

        //    // Assert
        //    Assert.IsInstanceOf<ContentResult>(result); // Check that the result is a ContentResult
        //    var contentResult = result as ContentResult;
        //    Assert.AreEqual("application/json", contentResult?.ContentType); // Ensure content type is JSON
        //    Assert.IsTrue(contentResult?.Content.Contains("Amount")); // Check that the payment amount is present in the JSON
        //}

        //[Test]
        //public async Task GetPaymentsByUserId_UserExistsWithNoPayments_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var user = new User { UserId = userId, FirstName = "Chris", Email = "chris@example.com", Role = "User" };
        //    var payments = new List<Payment>(); // Empty list for no payments

        //    // Mock the users DbSet to return the user when searched
        //    _mockUsersDbSet.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);

        //    // Mock the payments DbSet to return an empty list of payments
        //    _mockPaymentsDbSet.Setup(m => m.Where(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>())).Returns(payments.AsQueryable());

        //    // Act
        //    var result = await _controller.GetPaymentsByUserId(userId);

        //    // Assert
        //    Assert.IsInstanceOf<NotFoundObjectResult>(result); // Expect NotFoundObjectResult
        //    var notFoundResult = result as NotFoundObjectResult;
        //    Assert.AreEqual("No payments found for this user.", notFoundResult?.Value?.ToString()); // Check the message
        //}

        //[Test]
        //public async Task GetPaymentsByUserId_UserDoesNotExist_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var userId = 1;
        //    User user = null; // Simulate the user not being found

        //    // Mock the users DbSet to return null for the user search
        //    _mockUsersDbSet.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);

        //    // Act
        //    var result = await _controller.GetPaymentsByUserId(userId);

        //    // Assert
        //    Assert.IsInstanceOf<NotFoundObjectResult>(result); // Expect NotFoundObjectResult
        //    var notFoundResult = result as NotFoundObjectResult;
        //    Assert.AreEqual("User not found.", notFoundResult?.Value?.ToString()); // Check the message
        //}

    }
}
