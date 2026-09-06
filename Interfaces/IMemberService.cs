using GymSystem.Dtos;


namespace GymSystem.Interfaces
{
    public interface IMemberService
    {
        Task<MemberResponseDto> CreteAsync(CreateMemberDto dto);
        Task<MemberResponseDto> GetByIdAsync(int memberId);
        Task<MemberResponseDto> UpdateAsync(int memberId,UpdateMemberDto dto);
        Task DeleteAsync(int memberId);
        Task<PagedMemberResponseDto> GetAllAsync(MemberQueryDto query);


    }
}
