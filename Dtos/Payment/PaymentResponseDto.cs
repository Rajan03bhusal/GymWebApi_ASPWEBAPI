namespace GymSystem.Dtos.Payment
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int MemberMembershipId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public string PlanName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string? TransactionReference { get; set; }

        public string? Remarks { get; set; }
    }
}
