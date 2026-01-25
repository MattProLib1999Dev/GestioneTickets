using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestioneAccounts.BE.Domain.Models; // Assicurati che questo namespace sia corretto per le tue classi Account e Role

namespace GestioneTickets.DataAccess
{
    // Usiamo IdentityDbContext specificando le classi e il tipo della chiave primaria (int)
    public class ApplicationDbContext : IdentityDbContext<Account, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fondamentale: chiama il base.OnModelCreating per configurare le tabelle Identity
            base.OnModelCreating(builder);

            // Se hai relazioni specifiche, configurale qui. 
            // Esempio: configurazione per evitare cancellazioni a catena su MySQL se necessario
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // Aggiungi qui i tuoi DbSet per le altre tabelle
        public DbSet<Ticket> Tickets { get; set; }

        // NON inserire OnConfiguring con UseSqlServer qui dentro!
    }
}