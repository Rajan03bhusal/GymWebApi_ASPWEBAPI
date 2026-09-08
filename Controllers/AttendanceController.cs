using GymSystem.Dtos.Attendance;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _service;

        public AttendanceController(
            IAttendanceService service)
        {
            _service = service;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(
            CheckInAttendanceDto dto)
        {
            var result =
                await _service.CheckInAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.AttendanceId },
                result);
        }

        [HttpPost("check-out/{memberId}")]
        public async Task<IActionResult> CheckOut(
            int memberId)
        {
            var result =
                await _service.CheckOutAsync(memberId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] AttendanceQueryDto query)
        {
            var result =
                await _service.GetAllAsync(query);

            return Ok(result);
        }
    }
}