using GymSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GymSystem.Data
{
    public class AppDbContext :DbContext

    {
        public AppDbContext(DbContextOptions <AppDbContext> options) :base(options)
        {
            
        }
        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<MembershipPlan> MembershipPlans { get; set; }

        public DbSet<MemberMembership> MemberMemberships { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<MemberTrainer> MemberTrainers { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        }
}
