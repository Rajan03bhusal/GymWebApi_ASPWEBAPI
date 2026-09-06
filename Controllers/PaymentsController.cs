using GymSystem.Dtos.Payment;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePaymentDto dto)
        {
            var result =
                await _service.CreatePaymentAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.PaymentId },
                result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaymentQueryDto query)
        {
            var result =
                await _service.GetAllAsync(query);

            return Ok(result);
        }
    }
}