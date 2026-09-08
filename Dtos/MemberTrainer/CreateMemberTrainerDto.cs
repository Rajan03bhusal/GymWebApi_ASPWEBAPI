using System.ComponentModel.DataAnnotations;

namespace GymSystem.Dtos.MemberTrainer
{
    public class CreateMemberTrainerDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int TrainerId { get; set; }

       
    }
}
