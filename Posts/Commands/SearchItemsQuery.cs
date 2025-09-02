using MediatR; // Add this for IRequest<>
using GestioneAccounts.BE.Domain.Models; // Replace with the correct namespace for Account

public class SearchItemsQuery : IRequest<List<Account>>
{
    public string? Nome { get; set; } = string.Empty;
    public DateTime? DataCreazione { get; set; }
    public string? ValoreString { get; set; }
    public string? Voce { get; set; }
}
