using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GestioneTickets.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!;
        public DbSet<Account> Account { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Role)
                .WithMany(r => r.Tickets);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Tickets)
                .WithOne(t => t.Role);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Ticket)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
        }
    }
}
