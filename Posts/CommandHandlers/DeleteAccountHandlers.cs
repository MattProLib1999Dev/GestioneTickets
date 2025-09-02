using GestioneAccounts.Abstractions;  // Assicurati di avere la using corretta
using GestioneAccounts.Posts.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestioneAccounts.Posts.CommandHandlers
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccount, Unit>
    {
        private readonly IAccountRepository _accountRepository;

        // Cambia la dipendenza qui
        public DeleteAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(DeleteAccount request, CancellationToken cancellationToken)
        {
            // Qui dovrai chiamare il metodo per cancellare l'account
            await _accountRepository.DeleteAccount(request.Id.ToString());

            return Unit.Value;
        }
    }
}
