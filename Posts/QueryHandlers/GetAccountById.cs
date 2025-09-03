using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Queries;
using GestioneTickets.Abstractions;
using MediatR;

    public class GetAccountByIdHandler(ITicketRepository accountRepository) : IRequestHandler<GetTicketById, Ticket>
    {
            public readonly ITicketRepository _accountRepository = accountRepository;

        public async Task<Ticket> Handle(GetTicketById request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetTicketById(request.Id.ToString());
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {request.Id} not found.");
            }
            return account;
        }
    }
