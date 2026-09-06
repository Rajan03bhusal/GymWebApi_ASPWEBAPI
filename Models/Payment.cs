namespace GymSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int MemberMembershipId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string? TransactionReference { get; set; }

        public string? Remarks { get; set; }

        public MemberMembership? MemberMembership { get; set; }
    }
}
