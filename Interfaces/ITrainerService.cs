using GymSystem.Dtos.Trainer;

namespace GymSystem.Interfaces
{
    public interface ITrainerService
    {
        Task<TrainerResponseDto>
            CreateAsync(CreateTrainerDto dto);

        Task<TrainerResponseDto>
            GetByIdAsync(int id);

        Task<PagedTrainerResponseDto>
            GetAllAsync(TrainerQueryDto query);

        Task<TrainerResponseDto>
            UpdateAsync(int id, UpdateTrainerDto dto);

        Task DeleteAsync(int id);
    }
}
