using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface IMemberTrainerRepository
    {
        Task<MemberTrainer?> GetByIdAsync(int id);

        Task<MemberTrainer?>
            GetActiveAssignmentByMemberIdAsync(
                int memberId);

        Task<bool>
            ExistsActiveAssignmentAsync(
                int memberId);

        Task AddAsync(MemberTrainer memberTrainer);

        Task UpdateAsync(MemberTrainer memberTrainer);

        IQueryable<MemberTrainer> Query();

        Task<int> GetTotalCountAsync(
            IQueryable<MemberTrainer> query);

        Task<List<MemberTrainer>> GetPagedAsync(
            IQueryable<MemberTrainer> query,
            int pageNumber,
            int pageSize);

        Task SaveChangesAsync();
    }
}