using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class MemberTrainerRepository
        : IMemberTrainerRepository
    {
        private readonly AppDbContext _context;

        public MemberTrainerRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberTrainer?> GetByIdAsync(
            int id)
        {
            return await _context.MemberTrainers
                .Include(x => x.Member)
                .Include(x => x.Trainer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MemberTrainerId == id);
        }

        public async Task<MemberTrainer?>
            GetActiveAssignmentByMemberIdAsync(
                int memberId)
        {
            return await _context.MemberTrainers
                .Include(x => x.Member)
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(x =>
                    x.MemberId == memberId &&
                    x.IsActive);
        }

        public async Task<bool>
            ExistsActiveAssignmentAsync(
                int memberId)
        {
            return await _context.MemberTrainers
                .AnyAsync(x =>
                    x.MemberId == memberId &&
                    x.IsActive);
        }

        public async Task AddAsync(
            MemberTrainer memberTrainer)
        {
            await _context.MemberTrainers
                .AddAsync(memberTrainer);
        }

        public Task UpdateAsync(
            MemberTrainer memberTrainer)
        {
            _context.MemberTrainers
                .Update(memberTrainer);

            return Task.CompletedTask;
        }

        public IQueryable<MemberTrainer> Query()
        {
            return _context.MemberTrainers
                .AsNoTracking();
        }

        public async Task<int> GetTotalCountAsync(
            IQueryable<MemberTrainer> query)
        {
            return await query.CountAsync();
        }

        public async Task<List<MemberTrainer>>
            GetPagedAsync(
                IQueryable<MemberTrainer> query,
                int pageNumber,
                int pageSize)
        {
            return await query
                .Include(x => x.Member)
                .Include(x => x.Trainer)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}