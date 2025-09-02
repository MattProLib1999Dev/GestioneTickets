using GestioneTickets.DataAccess;
using GestioneAccounts.BE.Domain.Models;
using MediatR;
using GestioneTickets.Abstractions;

public class SearchAccountQueryHandler : IRequestHandler<SearchTicket, Ticket>
{
  public ITicketRepository _accountRepository { get; set; }
  public SearchAccountQueryHandler(ITicketRepository accountRepository)
  {
    _accountRepository = accountRepository;
  }

  // Inietta eventualmente il tuo servizio o contesto dati nel costruttore

  public async Task<Ticket> Handle(SearchTicket request, CancellationToken cancellationToken)
  {
        // Esegui la logica di ricerca. Esempio:
        var result = await _accountRepository.SearchTickets(
            nome: request.Nome,
            dataCreazione: request.DataCreazione,
            dataChiusura: request.DataChiusura,
            categoria: request.Categoria
        );

        // Restituisci il risultato della ricerca
        return result.FirstOrDefault() ?? new Ticket();
  }

}
