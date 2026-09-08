using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Repository
{
    public class AttendanceRepository
        : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<Attendance?> GetByIdAsync(
            int id)
        {
            return await _context.Attendances
                .Include(x => x.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.AttendanceId == id);
        }

        public async Task<Attendance?>
            GetTodayActiveAttendanceAsync(
                int memberId)
        {
            var today =
                DateTime.UtcNow.Date;

            return await _context.Attendances
                .FirstOrDefaultAsync(x =>
                    x.MemberId == memberId &&
                    x.AttendanceDate == today &&
                    x.CheckOutTime == null);
        }

        public async Task<bool> HasCheckedInTodayAsync(
            int memberId)
        {
            var today =
                DateTime.UtcNow.Date;

            return await _context.Attendances
                .AnyAsync(x =>
                    x.MemberId == memberId &&
                    x.AttendanceDate == today);
        }

        public async Task AddAsync(
            Attendance attendance)
        {
            await _context.Attendances
                .AddAsync(attendance);
        }

        public Task UpdateAsync(
            Attendance attendance)
        {
            _context.Attendances.Update(attendance);

            return Task.CompletedTask;
        }

        public IQueryable<Attendance> Query()
        {
            return _context.Attendances
                .AsNoTracking();
        }

        public async Task<int> GetTotalCountAsync(
            IQueryable<Attendance> query)
        {
            return await query.CountAsync();
        }

        public async Task<List<Attendance>>
            GetPagedAsync(
                IQueryable<Attendance> query,
                int pageNumber,
                int pageSize)
        {
            return await query
                .Include(x => x.Member)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}