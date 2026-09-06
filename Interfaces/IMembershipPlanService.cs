using GymSystem.Dtos.MembershipPlan;

namespace GymSystem.Interfaces
{
    public interface IMembershipPlanService
    {
        Task<MembershipPlanResponseDto> CreateAsync(
            CreateMembershipPlanDto dto);

        Task<MembershipPlanResponseDto> GetByIdAsync(
            int planId);

        Task<PagedMembershipPlanResponseDto> GetAllAsync(
            MembershipPlanQueryDto query);

        Task<MembershipPlanResponseDto> UpdateAsync(
            int planId,
            UpdateMembershipPlanDto dto);

        Task DeleteAsync(int planId);
    }
}
