using GestioneTickets.Abstractions;
using GestioneTickets.Model;
using MediatR;

namespace GestioneTickets.Posts.QueryHandlers
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
