using GymSystem.Dtos.MembershipPlan;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class MembershipPlanService : IMembershipPlanService
    {
        private readonly IMembershipPlanRepository
          _planRepository;
        public MembershipPlanService(IMembershipPlanRepository planRepository)
        {
          _planRepository = planRepository;   
        }


        // create
        public async Task<MembershipPlanResponseDto> CreateAsync(CreateMembershipPlanDto dto)
        {
            string planName =
               dto.PlanName.Trim();

            bool exists =
                await _planRepository
                    .ExistsByNameAsync(planName);
            if (exists)
            {
                throw new BadRequestException(
                    "A membership plan with this name already exists.");
            }
            var plan = new MembershipPlan
            {
                PlanName = planName,

                DurationInMonths =
                  dto.DurationInMonths,

                Price = dto.Price,

                Description = dto.Description?.Trim(),

                IsActive = true,


            };
            await _planRepository.AddAsync(plan);

            await _planRepository.SaveChangesAsync();
            return MapToResponse(plan);

        }

        public async Task DeleteAsync(int planId)
        {
            var plan =
                await _planRepository
                    .GetByIdAsync(planId);

            if (plan == null)
            {
                throw new NotFoundException(
                    $"Membership plan with ID {planId} was not found.");
            }

            if (plan.IsActive)
            {
                throw new BadRequestException(
                    "Active membership plans cannot be deleted. " +
                    "Deactivate the plan first.");
            }
            await _planRepository.DeleteAsync(plan);

            await _planRepository.SaveChangesAsync();

        }

        public async Task<PagedMembershipPlanResponseDto> GetAllAsync(MembershipPlanQueryDto query)
        {
            int totalRecords =
                await _planRepository
                    .GetTotalCountAsync(query);

            var plans =
                await _planRepository
                    .GetPagedAsync(query);

            var data = plans
                .Select(MapToResponse)
                .ToList();
            int totalPages =
              (int)Math.Ceiling(
                  totalRecords /
                  (double)query.PageSize);

            return new PagedMembershipPlanResponseDto
            {
                Data = data,

                PageNumber =
                    query.PageNumber,
                PageSize =
                    query.PageSize,

                TotalRecords =
                    totalRecords,

                TotalPages =
                    totalPages,

                HasPreviousPage =
                    query.PageNumber > 1,

                HasNextPage =
                    query.PageNumber < totalPages
            };


        }

        public async Task<MembershipPlanResponseDto> GetByIdAsync(int planId)
        {

            var plan =
                await _planRepository.GetByIdAsync(planId);

            if (plan == null)
            {
                throw new NotFoundException(
                    $"Membership plan with ID {planId} was not found.");
            }

            return MapToResponse(plan);
        }
         
        public async Task<MembershipPlanResponseDto> UpdateAsync(int planId, UpdateMembershipPlanDto dto)
        {
            var plan =
              await _planRepository
                  .GetByIdAsync(planId);

            if (plan == null)
            {
                throw new NotFoundException(
                    $"Membership plan with ID {planId} was not found.");
            }
            string planName =
              dto.PlanName.Trim();

            var existingPlan =
                await _planRepository
                    .GetByNameAsync(planName);

            if (existingPlan != null &&
                existingPlan.PlanId != planId)
            {
                throw new BadRequestException(
                    "Another membership plan already uses this name.");
            }
            plan.PlanName =
                          planName;

            plan.DurationInMonths =
                dto.DurationInMonths;

            plan.Price =
                dto.Price;

            plan.Description =
                dto.Description?.Trim();

            plan.IsActive =
                dto.IsActive;
            await _planRepository
                         .UpdateAsync(plan);

            await _planRepository
                .SaveChangesAsync();

            return MapToResponse(plan);
        }

        // MAPPING
        private static MembershipPlanResponseDto
            MapToResponse(MembershipPlan plan)
        {
            return new MembershipPlanResponseDto
            {
                PlanId =
                    plan.PlanId,

                PlanName =
                    plan.PlanName,

                DurationInMonths =
                    plan.DurationInMonths,

                Price =
                    plan.Price,

                Description =
                    plan.Description,

                IsActive =
                    plan.IsActive,

            };
        }
    }
}
