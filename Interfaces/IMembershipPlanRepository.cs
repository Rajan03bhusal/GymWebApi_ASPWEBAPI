using GymSystem.Dtos.MembershipPlan;
using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface IMembershipPlanRepository
    {
        Task<MembershipPlan?> GetByIdAsync(int planId);
        Task<MembershipPlan?> GetByNameAsync(string planName);
        Task<bool> ExistsByNameAsync(string planName);

        Task AddAsync(MembershipPlan plan);

        Task UpdateAsync(MembershipPlan plan);

        Task DeleteAsync(MembershipPlan plan);

        Task<int> GetTotalCountAsync(
           MembershipPlanQueryDto query);

        Task<List<MembershipPlan>> GetPagedAsync(
            MembershipPlanQueryDto query);

        Task SaveChangesAsync();
    }
}
