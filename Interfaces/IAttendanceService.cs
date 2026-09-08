using GymSystem.Dtos.Attendance;

namespace GymSystem.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto>
            CheckInAsync(CheckInAttendanceDto dto);

        Task<AttendanceResponseDto>
            CheckOutAsync(int memberId);

        Task<AttendanceResponseDto>
            GetByIdAsync(int id);

        Task<PagedAttendanceResponseDto>
            GetAllAsync(AttendanceQueryDto query);
    }
}