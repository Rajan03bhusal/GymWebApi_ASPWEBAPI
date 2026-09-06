using GymSystem.Dtos.MemberMembership;

namespace GymSystem.Interfaces
{
    public interface IMemberMembershipService
    {
        Task<MemberMembershipResponseDto> AssignMembershipAsync(CreateMemberMembershipDto dto); 
        Task<MemberMembershipResponseDto> GetByIdAsync(int id); 
        Task<PagedMemberMembershipResponseDto> GetAllAsync(MemberMembershipQueryDto query);
    }
}
