using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;

namespace GestioneAccounts.Abstractions
{
    public interface IAccountRepository
    {
        Task<ICollection<Ticket>> GetAllAccounts();
        Task<Ticket> GetAccountById(string accountId);
        Task<Ticket> CreateAccount(Ticket account);
        Task<Ticket> UpdateAccount(string? nome, int accountId);
        Task<bool> DeleteAccount(string accountId);
        Task<ICollection<Ticket>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce);

    }
}
