using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Queries;
using GestioneTickets.Abstractions;
using MediatR;
namespace GestioneTickets.Posts.QueryHandlers
{
    public class GetAccountById(IAccountRepository accountRepository) : IRequestHandler<GetTicketById, Account>
    {
        public readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<Account> Handle(GetTicketById request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountById(request.Id.ToString());
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {request.Id} not found.");
            }
            return account;
        }
    }
}
