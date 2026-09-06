using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class MemberTrainer
    {
        [Key]
        public int MemberTrainerId { get; set; }

        public int MemberId { get; set; }

        public int TrainerId { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public Member? Member { get; set; }

        public Trainer? Trainer { get; set; }
    }
}
