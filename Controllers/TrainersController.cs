using GymSystem.Dtos.Trainer;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/trainers")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _service;

        public TrainersController(
            ITrainerService service)
        {
            _service = service;
        }

        // POST: api/trainers
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTrainerDto dto)
        {
            var result =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.TrainerId },
                result);
        }

        // GET: api/trainers/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // GET: api/trainers
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] TrainerQueryDto query)
        {
            var result =
                await _service.GetAllAsync(query);

            return Ok(result);
        }

        // PUT: api/trainers/
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateTrainerDto dto)
        {
            var result =
                await _service.UpdateAsync(id, dto);

            return Ok(result);
        }

        // DELETE: api/trainers/
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}