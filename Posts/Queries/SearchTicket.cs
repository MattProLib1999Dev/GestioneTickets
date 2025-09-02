using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class SearchTicket : IRequest<Ticket>
{
    public string? Nome { get; set; } = string.Empty;
    public DateTime? DataCreazione { get; set; }
    public DateTime? DataChiusura { get; set; }
    public string? Categoria { get; set; }
}
