using MediatR;
using GestioneTickets.Model;
using System.Collections.Generic;
public class CreateRoleCommand : IRequest<Role>
{
    public int Id { get; set; }
    public string Roles { get; set; }  =  string.Empty;
    public string Name { get; set; } = String.Empty;
}
