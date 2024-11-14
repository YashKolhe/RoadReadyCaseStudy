using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cars = _carService.GetAllCars();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public IActionResult GetCarById(int id)
        {
            var car = _carService.GetCarById(id);
            if (car == null)
                return NotFound("Car not found");

            return Ok(car);
        }

        [HttpPost]
        public IActionResult Post(Car car)
        {
            var result = _carService.AddCar(car);
            return CreatedAtAction(nameof(GetCarById), new { id = result }, car);
        }

        [HttpPut]
        public IActionResult Put(Car car)
        {
            var result = _carService.UpdateCar(car);
            if (result == "Car not found")
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _carService.DeleteCar(id);
            if (result == "Car not found")
                return NotFound(result);

            return Ok(result);
        }
    }
}
