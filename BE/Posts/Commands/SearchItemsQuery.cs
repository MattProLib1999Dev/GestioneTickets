using MediatR; // Add this for IRequest<>
using GestioneTickets.Model;

public class SearchItemsQuery : IRequest<List<Ticket>>
{
    public string? Nome { get; set; } = string.Empty;
    public DateTime? DataCreazione { get; set; }
    public string? ValoreString { get; set; }
    public string? Voce { get; set; }
}
