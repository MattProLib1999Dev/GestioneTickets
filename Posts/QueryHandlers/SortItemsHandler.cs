using GestioneTickets.Abstractions;
using MediatR;
using GestioneTickets.Model;


public class SortAccountsHandler(ITicketRepository repository) : IRequestHandler<SortAccountsQuery, List<Ticket>>
{
    private readonly ITicketRepository _repository = repository;

  public async Task<List<Ticket>> Handle(SortAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetAllTicket();

        // Ordinamento dinamico
        return request.OrderBy switch
        {
            "Nome" => request.Descending ? [.. accounts.OrderByDescending(a => a.Nome)] : accounts.OrderBy(a => a.Nome).ToList(),
            "DataCreazione" => request.Descending ? [.. accounts.OrderByDescending(a => a.Nome)] : [.. accounts.OrderBy(a => a.Nome).ToList(),],
            "DataChiusura" => request.Descending ? [.. accounts.OrderByDescending(a => a.DataChiusura)] : [.. accounts.OrderBy(a => a.DataChiusura).ToList(),],
            "Categoria" => request.Descending ? [.. accounts.OrderByDescending(a => a.Categoria)] : [.. accounts.OrderBy(a => a.Categoria).ToList(),],
            _ => [.. accounts] // Se il campo OrderBy non è valido, ritorna la lista non ordinata
        };
    }
}

