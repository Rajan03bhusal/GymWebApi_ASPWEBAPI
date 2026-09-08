namespace GymSystem.Dtos.MemberTrainer
{
    public class MemberTrainerResponseDto
    {
        public int MemberTrainerId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public int TrainerId { get; set; }

        public string TrainerName { get; set; } = string.Empty;

        public DateTime AssignedDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
