using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<Account, Role, int>
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

    // Impedisci cascade path multipli
    foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        .SelectMany(e => e.GetForeignKeys()))
    {
        relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    // 1:N Account -> Tickets
    modelBuilder.Entity<Account>()
        .HasMany(a => a.Tickets)
        .WithOne(t => t.Account)
        .HasForeignKey(t => t.AccountId)
        .OnDelete(DeleteBehavior.Restrict);
}

}
