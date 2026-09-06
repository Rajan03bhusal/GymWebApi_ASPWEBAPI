namespace GymSystem.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MemberTrainer> MemberTrainers { get; set; }
           = new List<MemberTrainer>();
    }
}
