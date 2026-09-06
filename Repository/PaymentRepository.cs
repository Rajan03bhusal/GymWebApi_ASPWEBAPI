using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _dbContext;
        public PaymentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);
        }

        public async Task<bool> ExistsByTransactionReferenceAsync(string transactionReference)
        {
            return await _dbContext.Payments.AnyAsync(x => 
            x.TransactionReference == transactionReference );
        }

        public  async Task<Payment?> GetByIdAsync(int id)
        {
            return await _dbContext.Payments
                .Include( x=>x.MemberMembership)
                .ThenInclude( x => x.Member)
              .Include(x => x.MemberMembership)
                    .ThenInclude(x => x.Plan)
                     .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.PaymentId == id);
        }

        public async Task<List<Payment>> GetPagedAsync(IQueryable<Payment> query, int pageNumber, int pageSize)
        {
            return await query
                .Include(x => x.MemberMembership)
                    .ThenInclude(x => x.Member)
                .Include(x => x.MemberMembership)
                    .ThenInclude(x => x.Plan)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(IQueryable<Payment> query)
        {
            return await query.CountAsync();

        }

        public IQueryable<Payment> Query()
        {
            return _dbContext.Payments
                .AsNoTracking();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
