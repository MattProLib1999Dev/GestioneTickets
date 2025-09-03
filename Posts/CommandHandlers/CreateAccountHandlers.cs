using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Commands;
using GestioneTickets.Abstractions;
using MediatR;

namespace GestioneAccounts.Posts.CommandHandlers;
public class CreateAccountHandlers : IRequestHandler<CreateTicket, Ticket>
{
  private readonly ITicketRepository _ticketRepository;

  public CreateAccountHandlers(ITicketRepository accountRepository)
  {
      _ticketRepository = accountRepository;
  }

  public async Task<Ticket> Handle(CreateTicket request, CancellationToken cancellationToken)
  {
        var ticket = new Ticket
        {
            AccessFailedCount = 0,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Canc = request.Canc,
            Email = request.Email,
            EmailConfirmed = request.EmailConfirmed,
            Categoria = request.Categoria,
            DataChiusura = request.DataChiusura,
            DataCreazione = request.DataCreazione,
            Descrizione = request.Descrizione,
            LockoutEnabled = request.LockoutEnabled,
            LockoutEnd = request.LockoutEnd,
            Nome = request.Nome,
            NormalizedEmail = request.Email.ToUpper(),
            NormalizedUserName = request.Nome.ToUpper(),
            PasswordHash = request.PasswordHash,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = request.PhoneNumberConfirmed,
            SecurityStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = request.TwoFactorEnabled,
            UserName = request.Nome,
            Role = request.Role,
            ID_utente = request.ID_utente
        };
        return await _ticketRepository.CreateTicket(ticket);
  }
}
