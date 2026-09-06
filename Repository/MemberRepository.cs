using GymSystem.Data;
using GymSystem.Dtos;
using GymSystem.Interfaces.IMember;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymSystem.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _dbContext;

        public MemberRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Member member)
        {
            await _dbContext.Members.AddAsync(member);

        }

        public async Task DeleteAsync(Member member)
        {
            _dbContext.Members.Remove(member);

        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Members
               .AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
             return await _dbContext.Members
                .AnyAsync(x => x.PhoneNumber == phone);
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _dbContext.Members
             .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {

            return await _dbContext.Members
                .FirstOrDefaultAsync(x => x.MemberId == memberId);
        }

        public async Task<Member?> GetByPhoneAsync(string phone)
        {
            return await _dbContext.Members
               .FirstOrDefaultAsync(x => x.PhoneNumber == phone);
        }

       

        public async Task<int> GetTotalCountAsync(MemberQueryDto query)
        {
            IQueryable<Member> members=_dbContext.Members.AsNoTracking();
            members = ApplyFilters(members, query);

            return await members.CountAsync();
        }
        public async Task<List<Member>> GetPagedAsync(MemberQueryDto query)
        {

            IQueryable<Member> members =
                _dbContext.Members.AsNoTracking();

            members = ApplyFilters(members, query);

            members = ApplySorting(members, query);

            return await members
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(Member member)
        {
            _dbContext.Members.Update(member);

        }
        private static IQueryable<Member> ApplyFilters(IQueryable<Member> query, MemberQueryDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Search)){
                string search= filter.Search.Trim();
                query = query.Where(x =>
                x.MemberName.Contains(search)||
                x.Email.Contains(search) ||
                x.PhoneNumber.Contains(search) ||
                x.MemberCode.Contains(search)
                );
            }
            if (!string.IsNullOrWhiteSpace(filter.Gender))
            {
                query = query.Where(x =>
                    x.Gender == filter.Gender);
            }
            return query;

        }

        private static IQueryable<Member> ApplySorting(IQueryable<Member> query,
            MemberQueryDto filter)
        {
            bool descending = filter.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return filter.SortBy.ToLower() switch
            {

                "fullname" => descending
                    ? query.OrderByDescending(x => x.MemberName)
                    : query.OrderBy(x => x.MemberName),

                "email" => descending
                    ? query.OrderByDescending(x => x.Email)
                    : query.OrderBy(x => x.Email),
                "joindate" => descending
                ? query.OrderByDescending(x => x.JoinDate)
                : query.OrderBy(x => x.JoinDate),

                "gender" => descending
                    ? query.OrderByDescending(x => x.Gender)
                    : query.OrderBy(x => x.Gender),

                _ => descending
                    ? query.OrderByDescending(x => x.MemberId)
                    : query.OrderBy(x => x.MemberId)
            };
        }
    }
}
