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
        {
        }

        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(v => v.Role)
                .WithMany(r => r.Tickets);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Tickets)
                .WithOne(v => v.Role);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
        }
    }
}
