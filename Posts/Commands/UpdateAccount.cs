using GestioneAccounts.BE.Domain.Models;
using MediatR;
using GestioneTickets.Model;

public class UpdateAccount : IRequest<Account>
{
    public int Id { get; set; } 
    public string Nome { get; set; } = string.Empty;
}
