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
                Email = request.Email,
                Categoria = request.Categoria,
                DataCreazione = request.DataCreazione,
                DataChiusura = request.DataChiusura,
                AccountId = request.AccountId
            };

            await _ticketRepository.CreateTicket(ticket); 
            return ticket;
        }
    }
}
