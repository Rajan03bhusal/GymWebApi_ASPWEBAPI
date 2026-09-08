using GymSystem.Dtos.Attendance;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Interfaces.IMember;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberMembershipRepository _membershipRepository;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IMemberRepository memberRepository,
            IMemberMembershipRepository membershipRepository)
        {
            _attendanceRepository = attendanceRepository;
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<AttendanceResponseDto> CheckInAsync(
            CheckInAttendanceDto dto)
        {
            // 1. Check member exists
            var member = await _memberRepository
                .GetByIdAsync(dto.MemberId);

            if (member == null)
            {
                throw new NotFoundException(
                    "Member not found.");
            }

            //// 2. Check member is active
            //if (!member.IsActive)
            //{
            //    throw new BadRequestException(
            //        "Inactive member cannot check in.");
            //}

            // 3. Check active membership
            var activeMembership =
                await _membershipRepository
                    .GetActiveMembershipByMemberIdAsync(
                        dto.MemberId);

            if (activeMembership == null)
            {
                throw new BadRequestException(
                    "Member does not have an active membership.");
            }

            // 4. Check if member is currently checked in
            var activeAttendance =
                await _attendanceRepository
                    .GetTodayActiveAttendanceAsync(
                        dto.MemberId);

            if (activeAttendance != null)
            {
                throw new BadRequestException(
                    "Member is already checked in.");
            }

            // 5. Prevent second attendance record today
            var hasCheckedIn =
                await _attendanceRepository
                    .HasCheckedInTodayAsync(
                        dto.MemberId);

            if (hasCheckedIn)
            {
                throw new BadRequestException(
                    "Member has already completed attendance today.");
            }

            // 6. Create attendance
            var now = DateTime.UtcNow;

            var attendance = new Attendance
            {
                MemberId = dto.MemberId,
                AttendanceDate = now.Date,
                CheckInTime = now,
                CheckOutTime = null,
                Status = "Present",
                RecordedBy = null,
                CreatedAt = now
            };

            await _attendanceRepository
                .AddAsync(attendance);

            await _attendanceRepository
                .SaveChangesAsync();

            // 7. Return response
            return MapToResponse(attendance, member.MemberName);
        }

        public async Task<AttendanceResponseDto> CheckOutAsync(
            int memberId)
        {
            // Find today's active attendance
            var attendance =
                await _attendanceRepository
                    .GetTodayActiveAttendanceAsync(memberId);

            if (attendance == null)
            {
                throw new BadRequestException(
                    "Member has not checked in today.");
            }

            // Check out
            attendance.CheckOutTime = DateTime.UtcNow;
            attendance.Status = "Completed";

            await _attendanceRepository
                .UpdateAsync(attendance);

            await _attendanceRepository
                .SaveChangesAsync();

            return MapToResponse(
                attendance,
                attendance.Member?.MemberName ?? string.Empty);
        }

        public async Task<AttendanceResponseDto> GetByIdAsync(
            int id)
        {
            var attendance =
                await _attendanceRepository.GetByIdAsync(id);

            if (attendance == null)
            {
                throw new NotFoundException(
                    "Attendance record not found.");
            }

            return MapToResponse(
                attendance,
                attendance.Member.MemberName);
        }

        public async Task<PagedAttendanceResponseDto> GetAllAsync(
            AttendanceQueryDto query)
        {
            var attendanceQuery =
                _attendanceRepository.Query();

            // Member filter
            if (query.MemberId.HasValue)
            {
                attendanceQuery =
                    attendanceQuery.Where(x =>
                        x.MemberId == query.MemberId.Value);
            }

            // Date filters
            if (query.FromDate.HasValue)
            {
                attendanceQuery =
                    attendanceQuery.Where(x =>
                        x.AttendanceDate >=
                        query.FromDate.Value.Date);
            }

            if (query.ToDate.HasValue)
            {
                attendanceQuery =
                    attendanceQuery.Where(x =>
                        x.AttendanceDate <=
                        query.ToDate.Value.Date);
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                attendanceQuery =
                    attendanceQuery.Where(x =>
                        x.Status == query.Status);
            }

            // Sorting
            bool descending =
                query.SortOrder
                    .Equals(
                        "desc",
                        StringComparison.OrdinalIgnoreCase);

            attendanceQuery = query.SortBy.ToLower()
                switch
            {
                "attendancedate" => descending
                    ? attendanceQuery.OrderByDescending(
                        x => x.AttendanceDate)
                    : attendanceQuery.OrderBy(
                        x => x.AttendanceDate),

                "checkouttime" => descending
                    ? attendanceQuery.OrderByDescending(
                        x => x.CheckOutTime)
                    : attendanceQuery.OrderBy(
                        x => x.CheckOutTime),

                "status" => descending
                    ? attendanceQuery.OrderByDescending(
                        x => x.Status)
                    : attendanceQuery.OrderBy(
                        x => x.Status),

                _ => descending
                    ? attendanceQuery.OrderByDescending(
                        x => x.CheckInTime)
                    : attendanceQuery.OrderBy(
                        x => x.CheckInTime)
            };

            // Total records
            var totalRecords =
                await _attendanceRepository
                    .GetTotalCountAsync(attendanceQuery);

            // Pagination
            var attendanceList =
                await _attendanceRepository.GetPagedAsync(
                    attendanceQuery,
                    query.PageNumber,
                    query.PageSize);

            var data = attendanceList
                .Select(x => MapToResponse(
                    x,
                    x.Member.MemberName))
                .ToList();

            var totalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)query.PageSize);

            return new PagedAttendanceResponseDto
            {
                Data = data,
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

        private static AttendanceResponseDto MapToResponse(
            Attendance attendance,
            string memberName)
        {
            return new AttendanceResponseDto
            {
                AttendanceId =
                    attendance.AttendanceId,

                MemberId =
                    attendance.MemberId,

                MemberName =
                    memberName,

                AttendanceDate =
                    attendance.AttendanceDate,

                CheckInTime =
                    attendance.CheckInTime,

                CheckOutTime =
                    attendance.CheckOutTime,

                Status =
                    attendance.Status,

                RecordedBy =
                    attendance.RecordedBy,

                CreatedAt =
                    attendance.CreatedAt
            };
        }
    }
}