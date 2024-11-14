using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            User user = _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            int result = _userService.AddUser(user);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            string result = _userService.UpdateUser(user);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string result = _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}
