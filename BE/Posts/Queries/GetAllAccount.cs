using MediatR;

namespace GestioneTickets.Model
{
    public class GetAllAccount : IRequest<List<Account>>
    {
        public int AccountId { get; set; }
        public Role? Role { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Voce { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public DateTime DataCreazione { get; set; } = DateTime.Now;
        public double OreLavorate { get; set; } = 0.0;
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
        public string Password { get; set; } = string.Empty;
    }
}
