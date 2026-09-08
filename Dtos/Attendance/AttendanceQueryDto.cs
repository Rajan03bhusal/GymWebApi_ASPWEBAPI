using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Attendance
{
    public class AttendanceQueryDto
    {
        public int? MemberId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? Status { get; set; }

        public string SortBy { get; set; } = "CheckInTime";

        public string SortOrder { get; set; } = "desc";

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
