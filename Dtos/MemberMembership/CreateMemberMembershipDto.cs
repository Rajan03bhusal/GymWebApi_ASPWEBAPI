using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.MemberMembership
{
    public class CreateMemberMembershipDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}
