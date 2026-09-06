using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class MemberMembershipRepository : IMemberMembershipRepository
    {
        private readonly AppDbContext _dbContext;
        public MemberMembershipRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task AddAsync(MemberMembership membership)
        {
            await  _dbContext.AddAsync(membership);
        }

        public async Task<MemberMembership?> GetActiveMembershipByMemberIdAsync(int memberId)
        {
          return await _dbContext.MemberMemberships.FirstOrDefaultAsync( x=>
          x.MemberId == memberId && x.Status=="Active");
        }

        public async Task UpdateAsync(MemberMembership membership)
        {
            _dbContext.MemberMemberships.Update(membership);

        }

        public async Task<MemberMembership?> GetByIdAsync(int id)
        {
            return await _dbContext.MemberMemberships
               .Include(x => x.Member)
               .Include(x => x.Plan)
               .FirstOrDefaultAsync(x =>
                x.MemberMembershipId == id);

        }

        public async Task<List<MemberMembership>> GetPagedAsync(IQueryable<MemberMembership> query, int pageNumber, int pageSize)
        {
            return await query
              .Include(x => x.Member)
              .Include(x => x.Plan)
              .AsNoTracking()
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(IQueryable<MemberMembership> query)
        {
            return await query.CountAsync();
        }

        public IQueryable<MemberMembership> Query()
        {
            return _dbContext.MemberMemberships
                .Include(x => x.Member) 
                .Include(x => x.Plan) .
                AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();

        }
    }
}
