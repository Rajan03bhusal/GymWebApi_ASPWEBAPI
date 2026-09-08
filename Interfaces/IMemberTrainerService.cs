using GymSystem.Dtos.MemberTrainer;

namespace GymSystem.Interfaces
{
    public interface IMemberTrainerService
    {
        Task<MemberTrainerResponseDto>
            AssignTrainerAsync(
                CreateMemberTrainerDto dto);

        Task<MemberTrainerResponseDto>
            GetByIdAsync(int id);

        Task<PagedMemberTrainerResponseDto>
            GetAllAsync(
                MemberTrainerQueryDto query);

        Task EndAssignmentAsync(int id);
    }
}