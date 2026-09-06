using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos
{
    public class MemberQueryDto
    {
        public string? Search { get; set; }

        public string? Gender { get; set; }

        public bool? IsActive { get; set; }

        public string SortBy { get; set; } = "MemberId";

        public string SortOrder { get; set; } = "asc";

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
