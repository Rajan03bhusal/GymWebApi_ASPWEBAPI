using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);

        Task<bool> ExistsByTransactionReferenceAsync(
            string transactionReference);

        Task AddAsync(Payment payment);

        IQueryable<Payment> Query();

        Task<int> GetTotalCountAsync(
            IQueryable<Payment> query);

        Task<List<Payment>> GetPagedAsync(
            IQueryable<Payment> query,
            int pageNumber,
            int pageSize);

        Task SaveChangesAsync();
    }

}
