
using GymSystem.Dtos.MembershipPlan;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlansController : ControllerBase
    {
        private readonly IMembershipPlanService
            _planService;

        public MembershipPlansController(
            IMembershipPlanService planService)
        {
            _planService = planService;
        }


        // POST: api/membershipplans
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMembershipPlanDto dto)
        {
            var plan =
                await _planService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = plan.PlanId },
                plan);
        }


        // GET: api/membershipplans/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var plan =
                await _planService
                    .GetByIdAsync(id);

            return Ok(plan);
        }


        // GET: api/membershipplans
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] MembershipPlanQueryDto query)
        {
            var result =
                await _planService
                    .GetAllAsync(query);

            return Ok(result);
        }


        // PUT: api/membershipplans/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMembershipPlanDto dto)
        {
            var plan =
                await _planService
                    .UpdateAsync(id, dto);

            return Ok(plan);
        }


        // DELETE: api/membershipplans/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            await _planService
                .DeleteAsync(id);

            return NoContent();
        }
    }
}