using Microsoft.EntityFrameworkCore;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Infrastructure.Context
{
    public class VisitorManagementSystem : DbContext
    {
        public VisitorManagementSystem(DbContextOptions<VisitorManagementSystem> options) : base(options)
        {
            
        }

         public DbSet<Address> Addresses { get; set; }
         public DbSet<Profile> Profiles { get; set; }
         public DbSet<Role> Roles { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<UserRole> UserRoles { get; set; }
         public DbSet<Visitor> Visitors { get; set; }
         public DbSet<Visit> Visits { get; set; }
         public DbSet<AuditLog> AuditLogs { get; set; }
         public DbSet<Token> Tokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<Token>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<Profile>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<Visit>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<Role>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<User>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<UserRole>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<Visitor>()
            .HasKey(a => a.Id);
            modelBuilder.Entity<AuditLog>()
           .HasKey(a => a.Id);
            modelBuilder.Entity<Profile>()
        .HasKey(p => p.Id);

            
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .IsRequired(false);

        }
    }
}
