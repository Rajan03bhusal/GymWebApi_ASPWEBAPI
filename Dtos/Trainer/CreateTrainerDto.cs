using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.Trainer
{
    public class CreateTrainerDto
    {
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Specialization { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}
