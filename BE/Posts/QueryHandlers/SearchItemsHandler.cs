using GestioneTickets.Model;
using MediatR;
using GestioneTickets.Abstractions;

public class SearchAccountQueryHandler : IRequestHandler<SearchAccount, IEnumerable<Account>>
{
    public IAccountRepository _accountRepository { get; set; }
    public SearchAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // Inietta eventualmente il tuo servizio o contesto dati nel costruttore

    public async Task<IEnumerable<Account>> Handle(SearchAccount request, CancellationToken cancellationToken)
    {
        var result = await _accountRepository.SearchAccounts(request.Nome, request.DataCreazione, request.Categoria, request.Cognome);
        return result.FirstOrDefault(result.Select(a => new Account
        {
            Nome = request.Nome,
            Cognome = request.Cognome,
            DataCreazione = request.DataCreazione,
            DataChiusura = request.DataChiusura,
        })) ?? Enumerable.Empty<Account>()
                         .ToList();
    }

}
