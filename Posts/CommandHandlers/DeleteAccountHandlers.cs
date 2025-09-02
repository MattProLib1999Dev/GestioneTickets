using GestioneAccounts.Abstractions;  // Assicurati di avere la using corretta
using GestioneAccounts.Posts.Commands;
using GestioneTickets.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestioneAccounts.Posts.CommandHandlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteTicket, Unit>
    {
        private readonly ITicketRepository _accountRepository;

        // Cambia la dipendenza qui
        public DeleteAccountHandler(ITicketRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(DeleteTicket request, CancellationToken cancellationToken)
        {
            // Qui dovrai chiamare il metodo per cancellare l'account
            await _accountRepository.DeleteTicket(request.Id.ToString());

            return Unit.Value;
        }
    }
}
