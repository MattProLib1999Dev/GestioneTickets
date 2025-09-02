using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Abstractions;
using MediatR;

public class UpdateAccountHandler(ITicketRepository accountRepository) : IRequestHandler<UpdateTicket, Ticket>
{
    private readonly ITicketRepository _accountRepository = accountRepository;

  public async Task<Ticket> Handle(UpdateTicket request, CancellationToken cancellationToken)
    {
        // Verifica che l'ID non sia zero
        if (request.Id == null || request.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(request.Id), "Account Id cannot be null or zero.");
        }

        // Verifica che il nome non sia vuoto
        if (string.IsNullOrEmpty(request.Nome))
        {
            throw new ArgumentException("Account name cannot be null or empty.", nameof(request.Nome));
        }

        // A questo punto possiamo usare il repository per aggiornare l'account
        var accountModified = await _accountRepository.UpdateTicket(request.Nome, request.Id.GetHashCode())
            ?? throw new KeyNotFoundException("Account not found.");

        return accountModified;
    }

    


}
