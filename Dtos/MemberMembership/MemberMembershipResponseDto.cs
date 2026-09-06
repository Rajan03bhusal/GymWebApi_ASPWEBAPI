namespace GymSystem.Dtos.MemberMembership
{
    public class MemberMembershipResponseDto
    {
        public int MemberMembershipId { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;

        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
