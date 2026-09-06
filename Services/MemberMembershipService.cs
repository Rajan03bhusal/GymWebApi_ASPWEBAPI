
using GymSystem.Dtos.MemberMembership;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Interfaces.IMember;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class MemberMembershipService
        : IMemberMembershipService
    {
        private readonly IMemberMembershipRepository _repository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMembershipPlanRepository _planRepository;

        public MemberMembershipService(
            IMemberMembershipRepository repository,
            IMemberRepository memberRepository,
            IMembershipPlanRepository planRepository)
        {
            _repository = repository;
            _memberRepository = memberRepository;
            _planRepository = planRepository;
        }

        public async Task<MemberMembershipResponseDto>
            AssignMembershipAsync(
                CreateMemberMembershipDto dto)
        {
            // 1. Check member
            var member = await _memberRepository
                .GetByIdAsync(dto.MemberId);

            if (member == null)
            {
                throw new NotFoundException(
                    "Member not found.");
            }

           

            // 3. Check plan
            var plan = await _planRepository
                .GetByIdAsync(dto.PlanId);

            if (plan == null)
            {
                throw new NotFoundException(
                    "Membership plan not found.");
            }

            // 4. Check plan is active
            if (!plan.IsActive)
            {
                throw new BadRequestException(
                    "Cannot assign an inactive membership plan.");
            }

            // 5. Check existing active membership
            var existingMembership =
                await _repository
                    .GetActiveMembershipByMemberIdAsync(
                        dto.MemberId);

            if (existingMembership != null)
            {
                throw new BadRequestException(
                    "Member already has an active membership.");
            }

            // 6. Validate start date
            var startDate = dto.StartDate.Date;

            if (startDate < DateTime.UtcNow.Date)
            {
                throw new BadRequestException(
                    "Start date cannot be in the past.");
            }

            // 7. Calculate end date
            var endDate = startDate.AddMonths(
                plan.DurationInMonths);

            endDate = endDate.AddDays(-1);

            // 8. Create membership
            var membership = new MemberMembership
            {
                MemberId = dto.MemberId,
                PlanId = dto.PlanId,
                StartDate = startDate,
                EndDate = endDate,
                Amount = plan.Price,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(membership);

            await _repository.SaveChangesAsync();

            return new MemberMembershipResponseDto
            {
                MemberMembershipId =
                    membership.MemberMembershipId,

                MemberId = member.MemberId,
                MemberName = member.MemberName,

                PlanId = plan.PlanId,
                PlanName = plan.PlanName,

                StartDate = membership.StartDate,
                EndDate = membership.EndDate,

                Amount = membership.Amount,

                Status = membership.Status,

                CreatedAt = membership.CreatedAt
            };
        }

        public async Task<MemberMembershipResponseDto>
            GetByIdAsync(int id)
        {
            var membership =
                await _repository.GetByIdAsync(id);

            if (membership == null)
            {
                throw new NotFoundException(
                    "Membership not found.");
            }

            return MapToResponse(membership);
        }

        public async Task<PagedMemberMembershipResponseDto>
            GetAllAsync(MemberMembershipQueryDto query)
        {
            var memberships =
                BuildQuery(query);

            var totalRecords =
                await _repository.GetTotalCountAsync(
                    memberships);

            var data =
                await _repository.GetPagedAsync(
                    memberships,
                    query.PageNumber,
                    query.PageSize);

            var totalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)query.PageSize);

            return new PagedMemberMembershipResponseDto
            {
                Data = data
                    .Select(MapToResponse)
                    .ToList(),

                PageNumber = query.PageNumber,
                PageSize = query.PageSize,

                TotalRecords = totalRecords,
                TotalPages = totalPages,

                HasPreviousPage =
                    query.PageNumber > 1,

                HasNextPage =
                    query.PageNumber < totalPages
            };
        }

        private IQueryable<MemberMembership>
            BuildQuery(MemberMembershipQueryDto query)
        {
            var memberships =
                _repositoryQuery();

            if (query.MemberId.HasValue)
            {
                memberships = memberships.Where(x =>
                    x.MemberId == query.MemberId.Value);
            }

            if (query.PlanId.HasValue)
            {
                memberships = memberships.Where(x =>
                    x.PlanId == query.PlanId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                memberships = memberships.Where(x =>
                    x.Status == query.Status);
            }

            if (query.FromDate.HasValue)
            {
                memberships = memberships.Where(x =>
                    x.StartDate >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                memberships = memberships.Where(x =>
                    x.StartDate <= query.ToDate.Value);
            }

            memberships = query.SortBy.ToLower() switch
            {
                "enddate" =>
                    query.SortOrder.ToLower() == "desc"
                        ? memberships.OrderByDescending(x =>
                            x.EndDate)
                        : memberships.OrderBy(x =>
                            x.EndDate),

                "amount" =>
                    query.SortOrder.ToLower() == "desc"
                        ? memberships.OrderByDescending(x =>
                            x.Amount)
                        : memberships.OrderBy(x =>
                            x.Amount),

                "status" =>
                    query.SortOrder.ToLower() == "desc"
                        ? memberships.OrderByDescending(x =>
                            x.Status)
                        : memberships.OrderBy(x =>
                            x.Status),

                _ =>
                    query.SortOrder.ToLower() == "desc"
                        ? memberships.OrderByDescending(x =>
                            x.StartDate)
                        : memberships.OrderBy(x =>
                            x.StartDate)
            };

            return memberships;
        }

        private IQueryable<MemberMembership>
            _repositoryQuery()
        {
            return _repositoryQueryable;
        }

        private IQueryable<MemberMembership>
            _repositoryQueryable =>
            throw new NotImplementedException();

        private static MemberMembershipResponseDto
            MapToResponse(MemberMembership membership)
        {
            return new MemberMembershipResponseDto
            {
                MemberMembershipId =
                    membership.MemberMembershipId,

                MemberId = membership.MemberId,
                MemberName = membership.Member.MemberName,

                PlanId = membership.PlanId,
                PlanName = membership.Plan.PlanName,

                StartDate = membership.StartDate,
                EndDate = membership.EndDate,

                Amount = membership.Amount,

                Status = membership.Status,

                CreatedAt = membership.CreatedAt
            };
        }
    }
}