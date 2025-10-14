using GestioneAccounts.Posts.Commands;
using GestioneTickets.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestioneAccounts.Posts.CommandHandlers
{
    public class DeleteTicketHandler : IRequestHandler<DeleteTicket, Unit>
    {
        private readonly ITicketRepository _accountRepository;

        // Cambia la dipendenza qui
        public DeleteTicketHandler(ITicketRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(DeleteTicket request, CancellationToken cancellationToken)
        {
            // Qui dovrai chiamare il metodo per cancellare l'account
            await _accountRepository.DeleteTicket(request.Id);

            return Unit.Value;
        }
    }
}
