using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Payment
{
    public class PaymentQueryDto
    {
        public int? MemberId { get; set; }

        public int? MemberMembershipId { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string SortBy { get; set; } = "PaymentDate";

        public string SortOrder { get; set; } = "desc";

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
