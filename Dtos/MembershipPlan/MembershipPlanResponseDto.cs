namespace GymSystem.Dtos.MembershipPlan
{
    public class MembershipPlanResponseDto
    {
        public int PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public int DurationInMonths { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
