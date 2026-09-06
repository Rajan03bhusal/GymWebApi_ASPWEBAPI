using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Payment
{
    public class CreatePaymentDto
    {
        [Required]
        public int MemberMembershipId { get; set; }

        [Range(0.01, 999999999)]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}
