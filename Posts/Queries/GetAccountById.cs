using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using MediatR;

namespace GestioneAccounts.Posts.Queries
{
    public class GetAccountById : IRequest<Account>
    {
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public DateTime? DataCreazione { get; set; }
        public DateTime? DataChiusura { get; set; }
        public string? Categoria { get; set; }
    }
}
