using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using GestioneAccounts.Posts.Queries;
using MediatR;

    public class GetAccountByIdHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountById, Account>
    {
            public readonly IAccountRepository _accountRepository = accountRepository;

  public async Task<Account> Handle(GetAccountById request, CancellationToken cancellationToken)
            {
                if (!request.Id.HasValue)
                {
                    throw new ArgumentNullException(nameof(request.Id), "Account Id cannot be null.");
                }

                return await _accountRepository.GetAccountById(request.Id.Value.ToString());
            }

    }
