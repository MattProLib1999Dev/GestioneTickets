using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class UpdateAccountCommand : IRequest<Account>
{
    public int Id { get; set; }
    public string Nome { get; set; } = String.Empty;
    public List<Valore>? Valori { get; set; }
    public string valoreString { get; set; } = String.Empty;
    public string voce { get; set; } = String.Empty;
    public DateTime dataCreazione = DateTime.Now;
}
