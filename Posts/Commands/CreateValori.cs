using MediatR;

public class CreateValoreRequest(string accountId, string nome) : IRequest<Valori>
{
  public string AccountId { get; set; } = string.Empty;
  public string Nome { get; set; } = nome;
  public string? Descrizione { get; set; }
  public decimal? ValoreNumerico { get; set; }
}
