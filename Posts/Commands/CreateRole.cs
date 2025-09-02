using MediatR;
using GestioneTickets.Model;
public class CreateRoleCommand : IRequest<Role>
{
    public Guid AccountId { get; set; }
    public string Roles { get; set; }  =  string.Empty;
    public string Name { get; set; } = String.Empty;
}
