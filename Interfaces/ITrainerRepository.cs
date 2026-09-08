using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface ITrainerRepository
    {
        Task<Trainer?> GetByIdAsync(int id);

        Task<Trainer?> GetByEmailAsync(string email);

        Task<Trainer?> GetByPhoneAsync(string phone);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByPhoneAsync(string phone);

        Task AddAsync(Trainer trainer);
        Task UpdateAsync(Trainer trainer);

        IQueryable<Trainer> Query();

        Task<int> GetTotalCountAsync(
            IQueryable<Trainer> query);

        Task<List<Trainer>> GetPagedAsync(
            IQueryable<Trainer> query,
            int pageNumber,
            int pageSize);

        Task SaveChangesAsync();
   
    }
}
