using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class UpdateAccountHandler(IAccountRepository accountRepository) : IRequestHandler<UpdateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository = accountRepository;

  public async Task<Account> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        // Verifica che l'ID non sia zero
        if (request.Id == 0)
        {
            throw new ArgumentNullException(nameof(request.Id), "Account Id cannot be null or zero.");
        }

        // Verifica che il nome non sia vuoto
        if (string.IsNullOrEmpty(request.Nome))
        {
            throw new ArgumentException("Account name cannot be null or empty.", nameof(request.Nome));
        }

        // A questo punto possiamo usare il repository per aggiornare l'account
        var accountModified = await _accountRepository.UpdateAccount(request.Nome, request.Id)
            ?? throw new KeyNotFoundException("Account not found.");

        return accountModified;
    }
}
