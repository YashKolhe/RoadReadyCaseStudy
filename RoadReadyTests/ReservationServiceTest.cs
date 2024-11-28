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
    internal class ReservationServiceTest
    {
        private Mock<IReservationService> _mockReservationService;
        private ReservationController _controller;
        private Mock<ILogger<ReservationController>> _mockLogger;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _mockReservationService = new Mock<IReservationService>();
            _mockLogger = new Mock<ILogger<ReservationController>>();
            _controller = new ReservationController(_context,_mockReservationService.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAllReservations_ReturnsOkResult_WithListOfReservations()
        {
            // Arrange
            var reservations = new List<Reservation> {
                new Reservation { ReservationId = 1, PickupDate = DateTime.Parse("2023-11-24"), DropoffDate = DateTime.Parse("2023-11-29"), TotalAmount = 9999 },
                new Reservation { ReservationId = 2, PickupDate = DateTime.Parse("2023-11-14"), DropoffDate = DateTime.Parse("2023-11-18"), TotalAmount = 7999 }
            };
            _mockReservationService.Setup(service => service.GetAllReservations()).Returns(reservations);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(reservations, okResult?.Value);
        }

        [Test]
        public void GetReservationById_ReservationExists_ReturnsOkResult_WithReservation()
        {
            // Arrange
            var reservationId = 1;
            var reservation = new Reservation { ReservationId = reservationId, PickupDate = DateTime.Parse("2023-11-24"), DropoffDate = DateTime.Parse("2023-11-29"), TotalAmount = 9999 };
            _mockReservationService.Setup(service => service.GetReservationById(reservationId)).Returns(reservation);

            // Act
            var result = _controller.GetReservationById(reservationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(reservation, okResult?.Value);
        }

        [Test]
        public void AddReservation_ValidReservation_ReturnsCreatedAtAction()
        {
            // Arrange
            var newReservation = new Reservation { PickupDate = DateTime.Parse("2023-11-24"), DropoffDate = DateTime.Parse("2023-11-29"), TotalAmount = 9999 };
            _mockReservationService.Setup(service => service.AddReservation(newReservation)).Returns(newReservation.ReservationId);

            // Act
            var result = _controller.Post(newReservation);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(newReservation.ReservationId, createdResult?.RouteValues["id"]);
            Assert.AreEqual(newReservation, createdResult?.Value);
        }

        [Test]
        public void UpdateReservation_ValidIdAndReservation_ReturnsOkResult()
        {
            // Arrange
            var ReservationId = 1;
            var updatedReservation = new Reservation { ReservationId = ReservationId, PickupDate = DateTime.Parse("2023-11-14"), DropoffDate = DateTime.Parse("2023-11-18"), TotalAmount = 7999 };
            _mockReservationService.Setup(service => service.UpdateReservation(updatedReservation)).Returns("Reservation updated successfully");

            // Act
            var result = _controller.Put(updatedReservation);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Reservation updated successfully", okResult?.Value?.ToString());
        }

        [Test]
        public void DeleteReservation_ValidId_ReturnsOkResult()
        {
            // Arrange
            var reservationId = 1;
            _mockReservationService.Setup(service => service.DeleteReservation(reservationId)).Returns("Reservation deleted successfully");

            // Act
            var result = _controller.Delete(reservationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Reservation deleted successfully", okResult?.Value?.ToString());
        }
    }
}
