using GestioneTickets.Model;
namespace GestioneTickets.Abstractions
{
    public interface IAccountRepository
    {
        Task<ICollection<Account>> GetAllAccounts();
        Task<Account> GetAccountById(int accountId);
        Task<Account> CreateAccount(Account account);
        Task<Account> UpdateAccount(string? nome, int accountId);
        Task<bool> DeleteAccount(int accountId);
        Task<ICollection<IEnumerable<Account>>> SearchAccounts(string? nome, DateTime? dataCreazione, string? valoreString, string? voce);

    }
}
