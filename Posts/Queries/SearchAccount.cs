using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class SearchAccount : IRequest<Account>
{
    public string Nome { get; set; } = string.Empty;
  public DateTime DataCreazione { get; set; } = DateTime.Now;
  public string ValoreString { get; set; } = string.Empty;
}
