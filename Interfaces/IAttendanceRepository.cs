using GymSystem.Models;

namespace GymSystem.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);

        Task<Attendance?> GetTodayActiveAttendanceAsync(
            int memberId);

        Task<bool> HasCheckedInTodayAsync(int memberId);

        Task AddAsync(Attendance attendance);

        Task UpdateAsync(Attendance attendance);

        IQueryable<Attendance> Query();

        Task<int> GetTotalCountAsync(
            IQueryable<Attendance> query);

        Task<List<Attendance>> GetPagedAsync(
            IQueryable<Attendance> query,
            int pageNumber,
            int pageSize);
        Task SaveChangesAsync();
    }
}
