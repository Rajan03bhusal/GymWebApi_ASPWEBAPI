using GymSystem.Dtos.MemberTrainer;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Interfaces.IMember;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class MemberTrainerService
        : IMemberTrainerService
    {
        private readonly IMemberTrainerRepository _repository;
        private readonly IMemberRepository _memberRepository;
        private readonly ITrainerRepository _trainerRepository;

        public MemberTrainerService(
            IMemberTrainerRepository repository,
            IMemberRepository memberRepository,
            ITrainerRepository trainerRepository)
        {
            _repository = repository;
            _memberRepository = memberRepository;
            _trainerRepository = trainerRepository;
        }

        public async Task<MemberTrainerResponseDto>
            AssignTrainerAsync(
                CreateMemberTrainerDto dto)
        {
            //Check member
            var member =
                await _memberRepository
                    .GetByIdAsync(dto.MemberId);

            if (member == null)
            {
                throw new NotFoundException(
                    "Member not found.");
            }

           

            //  Check trainer
            var trainer =
                await _trainerRepository
                    .GetByIdAsync(dto.TrainerId);

            if (trainer == null)
            {
                throw new NotFoundException(
                    "Trainer not found.");
            }

            // 4. Trainer must be active
            if (!trainer.IsActive)
            {
                throw new BadRequestException(
                    "Inactive trainer cannot be assigned.");
            }

            //Check existing active assignment
            var existingAssignment =
                await _repository
                    .GetActiveAssignmentByMemberIdAsync(
                        dto.MemberId);

            if (existingAssignment != null)
            {
                throw new BadRequestException(
                    "Member already has an active trainer.");
            }

            //Create assignment
            var assignment = new MemberTrainer
            {
                MemberId = dto.MemberId,

                TrainerId = dto.TrainerId,

                AssignedDate =
                    DateTime.UtcNow,

                EndDate = null,

                IsActive = true,
            };

            await _repository.AddAsync(assignment);

            await _repository.SaveChangesAsync();

            //Return response
            return new MemberTrainerResponseDto
            {
                MemberTrainerId =
                    assignment.MemberTrainerId,

                MemberId =
                    member.MemberId,

                MemberName =
                    member.MemberName,

                TrainerId =
                    trainer.TrainerId,

                TrainerName =
                    trainer.FullName,

                AssignedDate =
                    assignment.AssignedDate,

                EndDate =
                    assignment.EndDate,

                IsActive =
                    assignment.IsActive,

             
            };
        }

        public async Task<MemberTrainerResponseDto>
            GetByIdAsync(int id)
        {
            var assignment =
                await _repository.GetByIdAsync(id);

            if (assignment == null)
            {
                throw new NotFoundException(
                    "Trainer assignment not found.");
            }

            return MapToResponseDto(assignment);
        }

        public async Task<PagedMemberTrainerResponseDto>
            GetAllAsync(
                MemberTrainerQueryDto query)
        {
            var assignments =
                _repository.Query();

            // Member filter
            if (query.MemberId.HasValue)
            {
                assignments = assignments.Where(x =>
                    x.MemberId ==
                    query.MemberId.Value);
            }

            // Trainer filter
            if (query.TrainerId.HasValue)
            {
                assignments = assignments.Where(x =>
                    x.TrainerId ==
                    query.TrainerId.Value);
            }

            // Active filter
            if (query.IsActive.HasValue)
            {
                assignments = assignments.Where(x =>
                    x.IsActive ==
                    query.IsActive.Value);
            }

            // From date
            if (query.FromDate.HasValue)
            {
                assignments = assignments.Where(x =>
                    x.AssignedDate >=
                    query.FromDate.Value);
            }

            // To date
            if (query.ToDate.HasValue)
            {
                assignments = assignments.Where(x =>
                    x.AssignedDate <=
                    query.ToDate.Value);
            }

            // Sorting
            var sortBy =
                query.SortBy?.ToLower();

            var descending =
                query.SortOrder?.ToLower() == "desc";

            assignments = sortBy switch
            {
                "memberid" =>
                    descending
                        ? assignments.OrderByDescending(
                            x => x.MemberId)
                        : assignments.OrderBy(
                            x => x.MemberId),

                "trainerid" =>
                    descending
                        ? assignments.OrderByDescending(
                            x => x.TrainerId)
                        : assignments.OrderBy(
                            x => x.TrainerId),

                "enddate" =>
                    descending
                        ? assignments.OrderByDescending(
                            x => x.EndDate)
                        : assignments.OrderBy(
                            x => x.EndDate),

                _ =>
                    descending
                        ? assignments.OrderByDescending(
                            x => x.AssignedDate)
                        : assignments.OrderBy(
                            x => x.AssignedDate)
            };

            // Total records
            var totalRecords =
                await _repository
                    .GetTotalCountAsync(assignments);

            // Pagination
            var assignmentList =
                await _repository.GetPagedAsync(
                    assignments,
                    query.PageNumber,
                    query.PageSize);

            var data =
                assignmentList
                    .Select(MapToResponseDto)
                    .ToList();

            var totalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)query.PageSize);

            return new PagedMemberTrainerResponseDto
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

        public async Task EndAssignmentAsync(int id)
        {
            var assignment =
                await _repository.GetByIdAsync(id);

            if (assignment == null)
            {
                throw new NotFoundException(
                    "Trainer assignment not found.");
            }

            if (!assignment.IsActive)
            {
                throw new BadRequestException(
                    "Trainer assignment is already inactive.");
            }

            assignment.IsActive = false;

            assignment.EndDate =
                DateTime.UtcNow;

            await _repository.UpdateAsync(
                assignment);

            await _repository.SaveChangesAsync();
        }

        private static MemberTrainerResponseDto
            MapToResponseDto(
                MemberTrainer assignment)
        {
            return new MemberTrainerResponseDto
            {
                MemberTrainerId =
                    assignment.MemberTrainerId,

                MemberId =
                    assignment.MemberId,

                MemberName =
                    assignment.Member.MemberName,

                TrainerId =
                    assignment.TrainerId,

                TrainerName =
                    assignment.Trainer.FullName,

                AssignedDate =
                    assignment.AssignedDate,

                EndDate =
                    assignment.EndDate,

                IsActive =
                    assignment.IsActive,

               
            };
        }
    }
}