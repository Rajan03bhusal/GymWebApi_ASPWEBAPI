using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos
{
    public class UpdateMemberDto
    {
        [Required]
      
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

      
        public string? Address { get; set; }
    }
}
