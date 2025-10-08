
using MediatR;
using GestioneTickets.Model;

public class GetAllAccount : IRequest<List<Account>>
{
    public Guid AccountId { get; set; } = Guid.Empty;
    public Role? Role { get; set; }
}
