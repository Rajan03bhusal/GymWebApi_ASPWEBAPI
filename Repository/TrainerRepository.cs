using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly AppDbContext _dbContext;

        public TrainerRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(Trainer trainer)
        {
            await _dbContext.AddAsync(trainer);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Trainers.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
           return await _dbContext.Trainers.AnyAsync(x => x.Phone == phone);
        }

        public async Task<Trainer?> GetByEmailAsync(string email)
        {
            return await _dbContext.Trainers
                   .FirstOrDefaultAsync(x =>
                       x.Email == email);
        }

        public async Task<Trainer?> GetByIdAsync(int id)
        {
            return await _dbContext.Trainers
                 .FirstOrDefaultAsync(x =>
                     x.TrainerId == id);
        }

        public async Task<Trainer?> GetByPhoneAsync(string phone)
        {
            return await _dbContext.Trainers
               .FirstOrDefaultAsync(x =>
                   x.Phone == phone);
        }

        public async Task<List<Trainer>> GetPagedAsync(IQueryable<Trainer> query, int pageNumber, int pageSize)
        {
            return await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(IQueryable<Trainer> query)
        {
            return await query.CountAsync();
        }

        public IQueryable<Trainer> Query()
        {
            return _dbContext.Trainers
               .AsNoTracking();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(Trainer trainer)
        {
            _dbContext.Trainers.Update(trainer);
        }
    }
}
