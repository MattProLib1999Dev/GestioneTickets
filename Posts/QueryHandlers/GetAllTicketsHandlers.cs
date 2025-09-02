using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using GestioneAccounts.Posts.Queries;
using GestioneTickets.Abstractions;
using MediatR;

namespace GestioneAccounts.Posts.QueryHandlers
{
  public class GetAllTicketsHandlers(ITicketRepository ticketRepository) : IRequestHandler<IRequest<ICollection<Ticket>>, ICollection<Ticket>>
  {
    public readonly ITicketRepository _ticketRepository = ticketRepository;

    public async Task<ICollection<Ticket>> Handle(IRequest<ICollection<Ticket>> request, CancellationToken cancellationToken)
    {
      return await _ticketRepository.GetAllTicket();
    }
  }
}
