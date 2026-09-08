using GymSystem.Dtos.Trainer;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _repository;
        public TrainerService(ITrainerRepository trainerRepository)
        {
            this._repository = trainerRepository;
        }
        public async Task<TrainerResponseDto> CreateAsync(CreateTrainerDto dto)
        {
            var email = dto.Email.Trim().ToLower();
            var phone = dto.Phone.Trim();

            // check duplicate email

            if (await _repository.ExistsByEmailAsync(email))
            {
                throw new Exception(
                    "A trainer with this email already exists"
                    );

            }
            // check duplicate phone

            if (await _repository.ExistsByPhoneAsync(phone))
            {
                throw new Exception(
                    "A trainer with this phone number already exists"
                    );

            }
            // Hire date cannot be in future
            if (dto.HireDate.Date >
                DateTime.UtcNow.Date)
            {
                throw new BadRequestException(
                    "Hire date cannot be in the future.");
            }
            var trainer = new Trainer
            {
                TrainerCode =
                   $"TRN-{Random.Shared.Next(1000, 9999)}",

                FullName = dto.FullName.Trim(),
                Gender =dto.Gender.Trim(),
                Phone = phone,
                Email = email,
                Specialization = string.IsNullOrWhiteSpace(
                        dto.Specialization)
                        ? null
                        : dto.Specialization.Trim(),
                HireDate = dto.HireDate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow

            };
            await _repository.AddAsync(trainer);

            await _repository.SaveChangesAsync();

            return MapToResponseDto(trainer);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedTrainerResponseDto> GetAllAsync(TrainerQueryDto query)
        {
            var trainers =
                _repository.Query();

            // Search
            if (!string.IsNullOrWhiteSpace(
                    query.Search))
            {
                var search =
                    query.Search.Trim();
                trainers = trainers.Where(x =>
                x.FullName.Contains(search) ||
                x.Email.Contains(search) ||
                x.Phone.Contains(search) ||
                (x.Specialization != null &&
                 x.Specialization.Contains(search)));
            }

            // Specialization filter
            if (!string.IsNullOrWhiteSpace(
                    query.Specialization))
            {
                var specialization =
                    query.Specialization.Trim();

                trainers = trainers.Where(x =>
                    x.Specialization == specialization);
            }

            // Active filter
            if (query.IsActive.HasValue)
            {
                trainers = trainers.Where(x =>
                    x.IsActive ==
                    query.IsActive.Value);
            }
            // Sorting
            var sortBy =
                query.SortBy?.ToLower();

            var descending =
                query.SortOrder?.ToLower() == "desc";

            trainers = sortBy switch
            {
                "fullname" =>
                    descending
                        ? trainers.OrderByDescending(
                            x => x.FullName)
                        : trainers.OrderBy(
                            x => x.FullName),
                "email" =>
        descending
            ? trainers.OrderByDescending(
                x => x.Email)
            : trainers.OrderBy(
                x => x.Email),

                "hiredate" =>
                    descending
                        ? trainers.OrderByDescending(
                            x => x.HireDate)
                        : trainers.OrderBy(
                            x => x.HireDate),

                "specialization" =>
                    descending
                        ? trainers.OrderByDescending(
                            x => x.Specialization)
                        : trainers.OrderBy(x => x.Specialization),

                _ =>
                    descending
                        ? trainers.OrderByDescending(
                            x => x.TrainerId)
                        : trainers.OrderBy(
                            x => x.TrainerId)
            };
            // Count
            var totalRecords =
                await _repository
                    .GetTotalCountAsync(trainers);

            // Pagination
            var trainerList =
                await _repository.GetPagedAsync(
                    trainers,
                    query.PageNumber,
                    query.PageSize);

            var data =
                trainerList
                    .Select(MapToResponseDto)
                    .ToList();
            var totalPages =
                           (int)Math.Ceiling(
                               totalRecords /
                               (double)query.PageSize);

            return new PagedTrainerResponseDto
            {
                Data = data,

                PageNumber =
                    query.PageNumber,

                PageSize =
                    query.PageSize,

                TotalRecords =
                    totalRecords,

                TotalPages = totalPages,

                HasPreviousPage =
                    query.PageNumber > 1,

                HasNextPage =
                    query.PageNumber < totalPages
            };
        }
        public async Task<TrainerResponseDto> GetByIdAsync(int id)
        {
            var trainer =
             await _repository.GetByIdAsync(id);

            if (trainer == null)
            {
                throw new NotFoundException(
                    "Trainer not found.");
            }

            return MapToResponseDto(trainer);
        }

        public async Task<TrainerResponseDto> UpdateAsync(int id, UpdateTrainerDto dto)
        {
            var trainer =
                await _repository.GetByIdAsync(id);

            if (trainer == null)
            {
                throw new NotFoundException(
                    "Trainer not found.");
            }

            var email =
                dto.Email.Trim().ToLower();

            var phone =
                dto.Phone.Trim();
            // Email belongs to another trainer
            var existingEmail =
                await _repository.GetByEmailAsync(email);

            if (existingEmail != null &&
                existingEmail.TrainerId != id)
            {
                throw new BadRequestException(
                    "A trainer with this email already exists.");
            }
            // Phone belongs to another trainer
            var existingPhone =
                await _repository.GetByPhoneAsync(phone);

            if (existingPhone != null &&
                existingPhone.TrainerId != id)
            {
                throw new BadRequestException(
                    "A trainer with this phone already exists.");
            }
            if (dto.HireDate.Date >
                          DateTime.UtcNow.Date)
            {
                throw new BadRequestException(
                    "Hire date cannot be in the future.");
            }

            trainer.FullName =
                dto.FullName.Trim();

            trainer.Gender =
                dto.Gender.Trim();

            trainer.Phone =
                phone;

            trainer.Email =
                email;
            trainer.Specialization =
                            string.IsNullOrWhiteSpace(
                                dto.Specialization)
                                ? null
                                : dto.Specialization.Trim();

            trainer.HireDate =
                dto.HireDate;

            trainer.IsActive =
                dto.IsActive;

            await _repository.UpdateAsync(trainer);

            await _repository.SaveChangesAsync();

            return MapToResponseDto(trainer);
        }

        // function
        private static TrainerResponseDto
          MapToResponseDto(Trainer trainer)
        {
            return new TrainerResponseDto
            {
                TrainerId =
                    trainer.TrainerId,

                TrainerCode =
                    trainer.TrainerCode,

                FullName = trainer.FullName,

                Gender =
                    trainer.Gender,

                Phone =
                    trainer.Phone,

                Email =
                    trainer.Email,

                Specialization =
                    trainer.Specialization,
                HireDate =
                    trainer.HireDate,

                IsActive =
                    trainer.IsActive,

                CreatedAt =
                    trainer.CreatedAt
            };


        }
    }
}
