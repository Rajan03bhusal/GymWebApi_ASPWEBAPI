namespace GymSystem.Dtos.Attendance
{
    public class AttendanceResponseDto
    {
        public int AttendanceId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public DateTime AttendanceDate { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public string Status { get; set; } = string.Empty;

        public int? RecordedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
