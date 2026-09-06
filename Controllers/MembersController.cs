
using GymSystem.Interfaces;
using GymSystem.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(
            IMemberService memberService)
        {
            _memberService = memberService;
        }


        // POST: api/members
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMemberDto dto)
        {
            var member =
                await _memberService.CreteAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = member.MemberId },
                member);
        }


   
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var member =
                await _memberService.GetByIdAsync(id);

            return Ok(member);
        }


       // GET: api/members
       [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] MemberQueryDto query)
        {
            var result =
                await _memberService.GetAllAsync(query);

            return Ok(result);
        }



        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMemberDto dto)
        {
            var member =
                await _memberService.UpdateAsync(id, dto);

            return Ok(member);
        }


        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            await _memberService.DeleteAsync(id);

            return NoContent();
        }
    }
}