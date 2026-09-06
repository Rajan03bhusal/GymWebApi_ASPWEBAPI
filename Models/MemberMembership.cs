using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class MemberMembership
    {
        [Key]
        public int MemberMembershipId { get; set; }

        public int MemberId { get; set; }

        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public Member? Member { get; set; }

        public MembershipPlan? Plan { get; set; }

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}
