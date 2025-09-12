using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class UpdateAccount : IRequest<Account>
{
    public int Id { get; set; } = 0;
    public string Nome { get; set; } = string.Empty;
}
