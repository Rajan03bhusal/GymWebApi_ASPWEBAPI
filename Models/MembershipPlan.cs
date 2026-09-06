using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class MembershipPlan
    {
        [Key]
        public int PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public int DurationInMonths { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MemberMembership> MemberMemberships { get; set; }
            = new List<MemberMembership>();
    }
}
