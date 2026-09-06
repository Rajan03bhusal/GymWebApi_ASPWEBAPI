using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }=string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;

        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleId {  get; set; }
        public Role ?Role { get; set; }


        
    }
}
