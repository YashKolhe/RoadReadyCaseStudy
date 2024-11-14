using RoadReady.Models;
using RoadReady.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound("Payment not found");

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult Post(Payment payment)
        {
            var result = _paymentService.AddPayment(payment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = result }, payment);
        }

        [HttpPut]
        public IActionResult Put(Payment payment)
        {
            var result = _paymentService.UpdatePayment(payment);
            if (result == "Payment not found")
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _paymentService.DeletePayment(id);
            if (result == "Payment not found")
                return NotFound(result);

            return Ok(result);
        }
    }
}
