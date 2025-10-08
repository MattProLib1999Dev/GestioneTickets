using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Commands;
using GestioneTickets.Abstractions; 
using MediatR;
using GestioneTickets.Model;

namespace GestioneTickets.Posts.CommandHandlers
{
    public class CreateTicketHandlers : IRequestHandler<CreateTicket, Ticket>
    {
        private readonly ITicketRepository _ticketRepository;

        public CreateTicketHandlers(ITicketRepository ticketRepository) // ✅ interfaccia
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> Handle(CreateTicket request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket
            {
                Id = request.Id,
                Nome = request.Nome,
                Password = request.Password,
                Email = request.Email,
                Role = request.Role

            };

            await _ticketRepository.CreateTicket(ticket); 
            return ticket;
        }
    }
}
