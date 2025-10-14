using GestioneTickets.Abstractions;
using GestioneTickets.Model;
using MediatR;

namespace GestioneTickets.Posts.QueryHandlers
{
  public class GetAllAccountHandlers(IAccountRepository accountRepository) : IRequestHandler<IRequest<ICollection<Account>>, ICollection<Account>>
  {
    public readonly IAccountRepository _ticketRepository = accountRepository;

    public async Task<ICollection<Account>> Handle(IRequest<ICollection<Account>> request, CancellationToken cancellationToken)
    {
      return await _ticketRepository.GetAllAccounts();
    }
  }
}
