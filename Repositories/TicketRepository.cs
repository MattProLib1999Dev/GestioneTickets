using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using MediatR;
using GestioneTickets.DataAccess; // Add the namespace containing ApplicationDbContext
using Microsoft.EntityFrameworkCore;

namespace GestioneAccounts.DataAccess.Repositories
{
  public class TicketRepository(ApplicationDbContext applicationDbContext) : IAccountRepository
  {
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Ticket> CreateAccount(Ticket account)
    {
      _applicationDbContext.Add(account);
      await _applicationDbContext.SaveChangesAsync();
      return account;
    }

    public async Task<bool> DeleteAccount(long accountId)
    {
      var account = await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id.ToString() == accountId.ToString());
      if (account == null)
        return false;

      _applicationDbContext.Tickets.Remove(account);
      await _applicationDbContext.SaveChangesAsync();
      return true;
    }

    public async Task<Ticket> GetAccountById(string accountId)
    {
      return await _applicationDbContext.Tickets.FirstOrDefaultAsync(a => a.Id.ToString() == accountId) ?? new Ticket();
    }


    public async Task<ICollection<Ticket>> GetAllAccounts()
    {
      return await _applicationDbContext.Tickets.ToListAsync();
    }
    // Removed duplicate UpdateAccount method to resolve conflict

    // search accounts by nome, dataCreazione, valoreString, voce
    public Task<ICollection<Ticket>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce)
    {
      var query = _applicationDbContext.Tickets.AsQueryable();
      if (!string.IsNullOrEmpty(nome))
        query = query.Where(a => a.Nome.Contains(nome));
      if (dataCreazione.HasValue)
        query = query.Where(a => a.DataCreazione == dataCreazione);
      if (!string.IsNullOrEmpty(valoreString))
        query = query.Where(a => a.ValoreString.Contains(valoreString));
      if (!string.IsNullOrEmpty(voce))
        query = query.Where(a => a.Voce.Contains(voce));

      return query.ToListAsync().ContinueWith(task => (ICollection<Ticket>)task.Result);
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
    public async Task<Ticket> UpdateAccount(string? nome, string accountId)
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
    public async Task<bool> DeleteAccount(string accountId)
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
