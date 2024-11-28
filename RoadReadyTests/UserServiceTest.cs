using NUnit.Framework;
using Moq;
using RoadReady.Controllers;
using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RoadReadyTests
{
    internal class UserServiceTest
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;
        private Mock<ILogger<UserController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockUserService.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAllusers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User> {
                new User { UserId = 1, FirstName = "Chris", Email = "chris@example.com", Role = "User" },
                new User { UserId = 2, FirstName = "Joe", Email = "joe@example.com", Role = "Admin" }
            };
            _mockUserService.Setup(service => service.GetAllUsers()).Returns(users);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(users, okResult?.Value);
        }

        [Test]
        public void GetUserById_UserExists_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, FirstName = "Chris", Email = "chris@example.com", Role = "User" };
            _mockUserService.Setup(service => service.GetUserById(userId)).Returns(user);

            // Act
            var result = _controller.GetUserById(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(user, okResult?.Value);
        }

        [Test]
        public void AddUser_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var newUser = new User { FirstName = "Sam", Email = "sam@example.com", Role = "User" };
            _mockUserService.Setup(service => service.AddUser(newUser)).Returns(newUser.UserId);

            // Act
            var result = _controller.Post(newUser);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(newUser.UserId, createdResult?.RouteValues["id"]);
            Assert.AreEqual(newUser, createdResult?.Value);
        }

        [Test]
        public void UpdateUser_ValidIdAndUser_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var updatedUser = new User { UserId = userId, FirstName = "Chris", Email = "chris@example.com", Role = "User" };
            _mockUserService.Setup(service => service.UpdateUser(updatedUser)).Returns("User updated successfully");

            // Act
            var result = _controller.Put(updatedUser);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("User updated successfully", okResult?.Value?.ToString());
        }

        [Test]
        public void DeleteUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            var UserId = 1;
            _mockUserService.Setup(service => service.DeleteUser(UserId)).Returns("User deleted successfully");

            // Act
            var result = _controller.Delete(UserId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("User deleted successfully", okResult?.Value?.ToString());
        }

    }
}
