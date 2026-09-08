using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Attendance
{
    public class CheckInAttendanceDto
    {
        [Required]
        public int MemberId { get; set; }
    }
}
