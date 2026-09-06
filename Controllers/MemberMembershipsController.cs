
using GymSystem.Dtos.MemberMembership;
using GymSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/membermemberships")]
    public class MemberMembershipsController : ControllerBase
    {
        private readonly IMemberMembershipService _service;

        public MemberMembershipsController(
            IMemberMembershipService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AssignMembership(
            CreateMemberMembershipDto dto)
        {
            var result =
                await _service.AssignMembershipAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.MemberMembershipId },
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
            [FromQuery] MemberMembershipQueryDto query)
        {
            var result =
                await _service.GetAllAsync(query);

            return Ok(result);
        }
    }
}