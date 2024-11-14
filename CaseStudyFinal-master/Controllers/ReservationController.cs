using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reservations = _reservationService.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = _reservationService.GetReservationById(id);
            if (reservation == null)
                return NotFound("Reservation not found");

            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult Post(Reservation reservation)
        {
            var result = _reservationService.AddReservation(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = result }, reservation);
        }

        [HttpPut]
        public IActionResult Put(Reservation reservation)
        {
            var result = _reservationService.UpdateReservation(reservation);
            if (result == "Reservation not found")
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _reservationService.DeleteReservation(id);
            if (result == "Reservation not found")
                return NotFound(result);

            return Ok(result);
        }
    }
}
