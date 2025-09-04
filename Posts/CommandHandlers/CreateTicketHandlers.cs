using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Commands;
using GestioneTickets.Abstractions; 
using MediatR;

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
                Titolo = request.Titolo,
                Descrizione = request.Descrizione,
                Categoria = request.Categoria,
                Role = request.Role,
                Nome = request.Nome,
                DataCreazione = request.DataCreazione,
                DataChiusura = request.DataChiusura,
                Canc = request.Canc,
                ID_utente = request.ID_utente
            };

            await _ticketRepository.CreateTicket(ticket); 
            return ticket;
        }
    }
}
