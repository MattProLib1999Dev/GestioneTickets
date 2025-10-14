using MediatR;
using GestioneTickets.Model;
using GestioneTickets.Repositories;
using GestioneTickets.Abstractions;

namespace GestioneTickets.Posts.CommandHandlers
{
    public class UpdateAccountHandler(IAccountRepository accountRepository) : IRequestHandler<UpdateAccount, Account>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<Account> Handle(UpdateAccount request, CancellationToken cancellationToken)
        {
            // Verifica che l'ID non sia vuoto
            if (request.Id == null)
            {
                throw new ArgumentException("Account Id cannot be empty.", nameof(request.Id));
            }

            // Verifica che il nome non sia vuoto
            if (string.IsNullOrWhiteSpace(request.Nome))
            {
                throw new ArgumentException("Account name cannot be null or empty.", nameof(request.Nome));
            }

            // Aggiorna l'account tramite repository
            var accountModified = await _accountRepository.UpdateAccount(request.Nome, request.Id)
                ?? throw new KeyNotFoundException("Account not found.");

            return accountModified;
        }
    }
}