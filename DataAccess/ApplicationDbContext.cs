using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


public class ApplicationDbContext : IdentityDbContext<Account, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) // ✅ qui chiami il costruttore corretto
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
}

