using GymSystem.Dtos;
using GymSystem.Models;

namespace GymSystem.Interfaces.IMember
{
    public interface IMemberRepository
    {
        Task<Member?> GetByIdAsync(int memberId);

        Task<Member?> GetByEmailAsync(string email);

        Task<Member?> GetByPhoneAsync(string phone);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByPhoneAsync(string phone);

        Task AddAsync(Member member);

        Task UpdateAsync(Member member);

        Task DeleteAsync(Member member);

        Task<int> GetTotalCountAsync(MemberQueryDto query);

        Task<List<Member>> GetPagedAsync(MemberQueryDto query);

        Task SaveChangesAsync();
    }
}