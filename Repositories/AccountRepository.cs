using GestioneTickets.Abstractions;
using GestioneTickets.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.DataAccess;
using AutoMapper;

namespace GestioneTickets.Repositories
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

    public async Task<bool> DeleteAccount(int accountId)
    {
      var account = await _applicationDbContext.Account.FirstOrDefaultAsync(a => a.Id == accountId);
      if (account == null)
        return false;

      _applicationDbContext.Account.Remove(account);
      await _applicationDbContext.SaveChangesAsync();
      return true;
    }

    public async Task<Account> GetAccountById(int accountId)
    {
      return await _applicationDbContext.Account.FirstOrDefaultAsync(a => a.Id == accountId) ?? new Account();
    }


    public async Task<ICollection<Account>> GetAllAccounts()
    {
      return await _applicationDbContext.Account.ToListAsync();
    }
    // Removed duplicate UpdateAccount method to resolve conflict

    // search accounts by nome, dataCreazione, valoreString, voce
    public async Task<ICollection<IEnumerable<Account>>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce)
    {
        var query = _applicationDbContext.Account.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(a => a.Nome.Contains(nome));
        }

        if (dataCreazione.HasValue)
        {
            query = query.Where(a => a.DataCreazione == dataCreazione.Value.Date);
        }

        var result = await query.ToListAsync();
        return new List<IEnumerable<Account>> { result };
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
  }
}
