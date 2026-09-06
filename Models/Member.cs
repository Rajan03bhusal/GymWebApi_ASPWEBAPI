using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        public string MemberCode { get; set; } = string.Empty;

        public string MemberName { get; set; }=string.Empty;

        public string Gender {  get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Address  { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<MemberMembership> MemberMemberships { get; set; }
           = new List<MemberMembership>();

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();

        public ICollection<MemberTrainer> MemberTrainers { get; set; }
            = new List<MemberTrainer>();




    }
}
