using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using GestioneAccounts.Posts.Queries;
using MediatR;

namespace GestioneAccounts.Posts.QueryHandlers
{
  public class GetAllaccountsHandlers(IAccountRepository accountRepository) : IRequestHandler<IRequest<ICollection<Account>>, ICollection<Account>>
  {
    public readonly IAccountRepository _accountRepository = accountRepository;

    public async Task<ICollection<Account>> Handle(IRequest<ICollection<Account>> request, CancellationToken cancellationToken)
    {
      return await _accountRepository.GetAllAccounts();
    }
  }
}
