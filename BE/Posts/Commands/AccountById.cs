using MediatR;
using GestioneTickets.Model;
public class AccountById : IRequest<GetAccountById>
{
    public int AccountId { get; set; } 
    public string Roles { get; set; }  =  string.Empty;
    public string Name { get; set; } = String.Empty;
}
