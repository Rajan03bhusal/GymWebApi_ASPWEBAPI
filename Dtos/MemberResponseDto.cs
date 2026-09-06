namespace GymSystem.Dtos
{
    public class MemberResponseDto
    {
        public int MemberId { get; set; }

        public string MemberCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Address { get; set; }

        public DateTime JoinDate { get; set; }

       
    }
}
