using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<MonthlyBudgetRun> MonthlyBudgetRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.User_Id, ur.Role_Id });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.User_Id);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.Role_Id);

            modelBuilder.Entity<Role>().HasData(
                new Role { Role_Id = 1, RoleName = "Admin" },
                new Role { Role_Id = 2, RoleName = "User"});

            base.OnModelCreating(modelBuilder);
        }
    }
}
