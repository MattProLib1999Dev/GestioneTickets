using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class SortAccountsQuery : IRequest<List<Account>>
{
    public string? Nome { get; set; }
    public DateTime? DataCreazione { get; set; }
    public string? ValoreString { get; set; }
    public string? Voce { get; set; }
    public string? OrderBy { get; set; } = "Nome"; // Campo di default per il sorting
    public bool Descending { get; set; } = false;  // Ordinamento crescente o decrescente
}
