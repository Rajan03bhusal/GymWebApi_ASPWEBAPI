namespace GymSystem.Dtos.Trainer
{
    public class TrainerResponseDto
    {
        public int TrainerId { get; set; }

        public string TrainerCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Specialization { get; set; }

        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
