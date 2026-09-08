using GymSystem.Dtos.MemberTrainer;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/membertrainers")]
    public class MemberTrainersController
        : ControllerBase
    {
        private readonly IMemberTrainerService _service;

        public MemberTrainersController(
            IMemberTrainerService service)
        {
            _service = service;
        }

        // POST: api/membertrainers
        [HttpPost]
        public async Task<IActionResult> AssignTrainer(
            CreateMemberTrainerDto dto)
        {
            var result =
                await _service.AssignTrainerAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = result.MemberTrainerId
                },
                result);
        }

        // GET: api/membertrainers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // GET: api/membertrainers
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] MemberTrainerQueryDto query)
        {
            var result =
                await _service.GetAllAsync(query);

            return Ok(result);
        }

        // PUT: api/membertrainers/{id}/end
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndAssignment(
            int id)
        {
            await _service.EndAssignmentAsync(id);

            return NoContent();
        }
    }
}