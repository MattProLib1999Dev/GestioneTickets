using GestioneTickets.Model;
using MediatR;

public class SearchAccount : IRequest<IEnumerable<Account>>
{
    public string? Nome { get; set; }
    public string? Cognome { get; set; }
    public DateTime DataCreazione { get; set; } = DateTime.Today;
    public DateTime? DataChiusura { get; set; }
    public string? Categoria { get; set; }
}
