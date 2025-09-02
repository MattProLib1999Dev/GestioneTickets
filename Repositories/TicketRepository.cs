using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using MediatR;
using GestioneTickets.DataAccess; // Add the namespace containing ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using GestioneTickets.Abstractions;

namespace GestioneAccounts.DataAccess.Repositories
{
  public class TicketRepository(ApplicationDbContext applicationDbContext) : ITicketRepository
  {
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Ticket> CreateTicket(Ticket account)
    {
      _applicationDbContext.Add(account);
      await _applicationDbContext.SaveChangesAsync();
      return account;
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
    public async Task<ICollection<Ticket>> SearchTickets(
    string? nome, DateTime? dataCreazione, DateTime? dataChiusura, string? categoria)
{
    var query = _applicationDbContext.Tickets.AsQueryable();

    if (!string.IsNullOrEmpty(nome))
        query = query.Where(a => a.Nome.Contains(nome));

    if (dataCreazione.HasValue)
        query = query.Where(a => a.DataCreazione == dataCreazione.Value.Date);

    if (dataChiusura.HasValue)
    {
        query = query.Where(a => a.DataChiusura.HasValue && 
                                a.DataChiusura.Value.Date == dataChiusura.Value.Date);
    }


    if (categoria != null && Enum.TryParse<Categoria>(categoria, out var categoriaValue))
        query = query.Where(a => a.Categoria == categoriaValue);

    return await query.ToListAsync();
}


    public async Task<Ticket> UpdateAccount(string? nome, int accountId)
    {
      var id = new Guid();
      var existingAccount = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == id);
      if (existingAccount == null)
        return new Ticket { Id = id, Nome = nome ?? string.Empty };

      existingAccount.Nome = nome ?? existingAccount.Nome;
      _applicationDbContext.Tickets.Update(existingAccount);
      await _applicationDbContext.SaveChangesAsync();
      return existingAccount;
    }
    public async Task<Ticket> UpdateTicket(string? nome, int ticketId)
    {
      var id = new Guid();
      var existingAccount = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == id);
      if (existingAccount == null)
        return new Ticket { Id = id, Nome = nome ?? string.Empty };

      existingAccount.Nome = nome ?? existingAccount.Nome;
      _applicationDbContext.Tickets.Update(existingAccount);
      await _applicationDbContext.SaveChangesAsync();
      return existingAccount;
    }
    public async Task<bool> DeleteTicket(string accountId)
    {
      var id = new Guid();
      var account = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id == id);
      if (account == null)
        return false;

      _applicationDbContext.Tickets.Remove(account);
      await _applicationDbContext.SaveChangesAsync();
      return true;


    }
  }
}
