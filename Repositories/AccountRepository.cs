using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestioneAccounts.DataAccess.Repositories
{
  public class AccountRepository(ApplicationDbContext applicationDbContext) :  IAccountRepository
  {
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Account> CreateAccount(Account account)
    {
      _applicationDbContext.Add(account);
      await _applicationDbContext.SaveChangesAsync();
      return account;
    }

    public async Task<bool> DeleteAccount(long accountId)
    {
      var account = await _applicationDbContext.Account.FirstOrDefaultAsync(a => a.Id.ToString() == accountId.ToString());
      if (account == null)
        return false;

      _applicationDbContext.Account.Remove(account);
      await _applicationDbContext.SaveChangesAsync();
      return true;
    }

    public async Task<Account> GetAccountById(string accountId)
    {
      return await _applicationDbContext.Account.FirstOrDefaultAsync(a => a.Id.ToString() == accountId) ?? new Account();
    }


    public async Task<ICollection<Account>> GetAllAccounts()
    {
      return await _applicationDbContext.Account.ToListAsync();
    }
    // Removed duplicate UpdateAccount method to resolve conflict

    // search accounts by nome, dataCreazione, valoreString, voce
    public Task<ICollection<Account>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce)
    {
      var query = _applicationDbContext.Account.AsQueryable();
      if (!string.IsNullOrEmpty(nome))
        query = query.Where(a => a.Nome.Contains(nome));
      if (dataCreazione.HasValue)
        query = query.Where(a => a.DataCreazione == dataCreazione);
      if (!string.IsNullOrEmpty(valoreString))
        query = query.Where(a => a.ValoreString.Contains(valoreString));
      if (!string.IsNullOrEmpty(voce))
        query = query.Where(a => a.Voce.Contains(voce));

      return query.ToListAsync().ContinueWith(task => (ICollection<Account>)task.Result);
    }

    public async Task<Account> UpdateAccount(string? nome, int accountId)
    {
        var existingAccount = await _applicationDbContext.Account
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (existingAccount == null)
        {
            // Se vuoi creare un nuovo account se non esiste:
            var newAccount = new Account
            {
                Id = accountId,
                Nome = nome ?? string.Empty
            };
            _applicationDbContext.Account.Add(newAccount);
            await _applicationDbContext.SaveChangesAsync();
            return newAccount;
        }

        // Aggiorna il nome esistente
        existingAccount.Nome = nome ?? existingAccount.Nome;
        _applicationDbContext.Account.Update(existingAccount);
        await _applicationDbContext.SaveChangesAsync();
        return existingAccount;
    }

    public async Task<bool> DeleteAccount(int accountId)
    {
      var id = new Guid();
      var account = await _applicationDbContext.Account.FirstOrDefaultAsync(a => a.Id == accountId);
      if (account == null)
        return false;

      _applicationDbContext.Account.Remove(account);
      await _applicationDbContext.SaveChangesAsync();
      return true;


    }
  }
}
