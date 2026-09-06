using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.MembershipPlan
{
    public class UpdateMembershipPlanDto
    {
        [Required]
        [MaxLength(100)]
        public string PlanName { get; set; } = string.Empty;

        [Range(1, 120)]
        public int DurationInMonths { get; set; }

        [Range(0.01, 999999)]
        public decimal Price { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
