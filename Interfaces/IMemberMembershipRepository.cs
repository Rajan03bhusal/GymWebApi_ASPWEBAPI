using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface IMemberMembershipRepository
    {
        Task<MemberMembership?> GetByIdAsync(int id);

        Task<MemberMembership?> GetActiveMembershipByMemberIdAsync(
            int memberId);
        Task UpdateAsync(MemberMembership membership);
        IQueryable<MemberMembership> Query();

        Task AddAsync(MemberMembership membership);

        Task<int> GetTotalCountAsync(
            IQueryable<MemberMembership> query);

        Task<List<MemberMembership>> GetPagedAsync(
            IQueryable<MemberMembership> query,
            int pageNumber,
            int pageSize);

        Task SaveChangesAsync();
    }
}
