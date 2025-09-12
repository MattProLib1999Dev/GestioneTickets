using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;

namespace GestioneAccounts.Abstractions
{
    public interface IAccountRepository
    {
        Task<ICollection<Account>> GetAllAccounts();
        Task<Account> GetAccountById(string accountId);
        Task<Account> CreateAccount(Account account);
        Task<Account> UpdateAccount(string? nome, int accountId);
        Task<bool> DeleteAccount(int accountId);
        Task<ICollection<Account>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce);

    }
}
