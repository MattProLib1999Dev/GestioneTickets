using Microsoft.EntityFrameworkCore;
using GestioneTickets.Abstractions;
using GestioneTickets.Model;

namespace GestioneTickets.Repositories
{
    public class TicketRepository(ApplicationDbContext applicationDbContext) : ITicketRepository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<Ticket?> CreateTicket(Ticket ticket)
        {
            try
            {
                // Verifica se l'AccountId esiste nel DB
                var accountExists = await _applicationDbContext.Account
                    .AnyAsync(u => u.Id == ticket.Id);

                if (!accountExists)
                {
                    // Account non trovato: ritorna null o gestisci diversamente
                    return null;
                }

                _applicationDbContext.Tickets.Add(ticket);
                await _applicationDbContext.SaveChangesAsync();

                return ticket;
            }
            catch (DbUpdateException ex)
            {
                // Log dell'errore
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<bool> DeleteTicket(long accountId)
        {
            var account = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id.ToString() == accountId.ToString());
            if (account == null)
                return false;

            _applicationDbContext.Tickets.Remove(account);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Ticket> GetTicketById(string ticketId)
        {
            return await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id.ToString() == ticketId) ?? new Ticket();
        }


        public async Task<ICollection<Ticket>> GetAllTicket()
        {
            return await _applicationDbContext.Tickets.ToListAsync();
        }
        // Removed duplicate UpdateAccount method to resolve conflict

        // search accounts by nome, dataCreazione, valoreString, voce
        public async Task<ICollection<Ticket>> SearchTickets(string? nome, string? ticketId, DateTime? dataCreazione, DateTime? dataChiusura, string? categoria)
        {
            var query = _applicationDbContext.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(a => a.Nome.Contains(nome));
            }

            if (dataCreazione.HasValue)
            {
                query = query.Where(a => a.DataCreazione == dataCreazione);
            }

            if (dataChiusura.HasValue)
            {
                query = query.Where(a => a.DataChiusura == dataChiusura);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(a => a.Categoria.ToString().Contains(categoria));
            }

            return await query.ToListAsync();
        }


        public async Task<Ticket> UpdateAccount(string? nome, int accountId)
        {
            var existingAccount = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == accountId);
            if (existingAccount == null)
                return new Ticket { Id = accountId, Nome = nome ?? string.Empty };

            existingAccount.Nome = nome ?? existingAccount.Nome;
            _applicationDbContext.Tickets.Update(existingAccount);
            await _applicationDbContext.SaveChangesAsync();
            return existingAccount;
        }
        public async Task<Ticket> UpdateTicket(string? nome, int ticketId)
        {
            var existingAccount = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == ticketId);
            if (existingAccount == null)
                return new Ticket { Id = ticketId, Nome = nome ?? string.Empty };

            existingAccount.Nome = nome ?? existingAccount.Nome;
            _applicationDbContext.Tickets.Update(existingAccount);
            await _applicationDbContext.SaveChangesAsync();
            return existingAccount;
        }
        public async Task<bool> DeleteTicket(int accountId)
        {
            var id = new Guid();
            var account = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
                return false;

            _applicationDbContext.Tickets.Remove(account);
            await _applicationDbContext.SaveChangesAsync();
            return true;


        }
    }
}