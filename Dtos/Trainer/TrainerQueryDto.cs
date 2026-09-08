using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Trainer
{
    public class TrainerQueryDto
    {
        public string? Search { get; set; }

        public string? Specialization { get; set; }

        public bool? IsActive { get; set; }

        public string SortBy { get; set; } = "TrainerId";

        public string SortOrder { get; set; } = "asc";

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
