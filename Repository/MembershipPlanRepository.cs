using GymSystem.Data;
using GymSystem.Dtos.MembershipPlan;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class MembershipPlanRepository : IMembershipPlanRepository
    {
        private readonly AppDbContext _dbContext;
        public MembershipPlanRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(MembershipPlan plan)
        {
            await _dbContext.MembershipPlans
               .AddAsync(plan);
        }

        public async Task DeleteAsync(MembershipPlan plan)
        {
            _dbContext.MembershipPlans.Remove(plan);


        }

        public async Task<bool> ExistsByNameAsync(string planName)
        {
            return await _dbContext.MembershipPlans.AnyAsync(x => x.PlanName == planName);
        }

        public async Task<MembershipPlan?> GetByIdAsync(int planId)
        {
            return await _dbContext.MembershipPlans.FirstOrDefaultAsync(x => x.PlanId == planId);

        }

        public async Task<MembershipPlan?> GetByNameAsync(string planName)
        {
            return await _dbContext.MembershipPlans.FirstOrDefaultAsync
                (x => x.PlanName == planName);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(MembershipPlan plan)
        {
            _dbContext.MembershipPlans.Update(plan);
        }

        public async Task<List<MembershipPlan>> GetPagedAsync(MembershipPlanQueryDto query)
        {

            IQueryable<MembershipPlan> plans =
                _dbContext.MembershipPlans.AsNoTracking();

            plans = ApplyFilters(plans, query);
            plans = ApplySorting(plans, query);

            return await plans
             .Skip(
                 (query.PageNumber - 1)
                 * query.PageSize)
             .Take(query.PageSize)
             .ToListAsync();
        }


        public async Task<int> GetTotalCountAsync(MembershipPlanQueryDto query)
        {
            IQueryable<MembershipPlan> plans =
                 _dbContext.MembershipPlans.AsNoTracking();

            plans = ApplyFilters(plans, query);

            return await plans.CountAsync();
        }

        private static IQueryable<MembershipPlan> ApplyFilters(IQueryable<MembershipPlan> query,
            MembershipPlanQueryDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string search = filter.Search.Trim();
                query = query.Where(x =>
                   x.PlanName.Contains(search) ||
                     (x.Description != null &&
                     x.Description.Contains(search)));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.HasValue);

            }
            return query;
        }
        private static IQueryable<MembershipPlan> ApplySorting(
           IQueryable<MembershipPlan> query,
           MembershipPlanQueryDto filter)
        {
            bool descending =
                filter.SortOrder.Equals(
                    "desc",
                    StringComparison.OrdinalIgnoreCase);

            return filter.SortBy.ToLower() switch
            {
                "planname" => descending
                    ? query.OrderByDescending(x => x.PlanName)
                    : query.OrderBy(x => x.PlanName),

                "duration" => descending
                 ? query.OrderByDescending(
                        x => x.DurationInMonths)
                    : query.OrderBy(
                        x => x.DurationInMonths),

                "price" => descending
                    ? query.OrderByDescending(x => x.Price)
                    : query.OrderBy(x => x.Price),

                _ => descending
                    ? query.OrderByDescending(x => x.PlanId)
                    : query.OrderBy(x => x.PlanId)
            };
        }
    }
}