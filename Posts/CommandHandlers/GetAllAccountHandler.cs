using GestioneTickets.Model;
using GestioneTickets.Abstractions;
using MediatR;


public class GetAllAccountHandler(IAccountRepository accountRepository) : IRequestHandler<GetAllAccount, IEnumerable<Account>>
{
    private readonly IAccountRepository _accountRepository = accountRepository;

    public async Task<IEnumerable<Account>> Handle(GetAllAccount request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAccounts();
        return accounts;
    }
}