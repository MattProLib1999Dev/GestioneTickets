using GestioneTickets.Model;
using MediatR;

public class SearchTicket : IRequest<IEnumerable<Account>>
{
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public DateTime DataCreazione { get; set; } = DateTime.Now;
    public double OreLavorate { get; set; } = 0.0;
    public string Password { get; set; } = string.Empty;
}
