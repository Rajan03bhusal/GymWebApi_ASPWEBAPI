namespace GymSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int MemberId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public Member? Member { get; set; }
    }
}
