using GestioneTickets.Model;
using MediatR;

namespace GestioneTickets.Posts.Queries
{
	public class GetTicketById: IRequest<Account>
	{
        public long Id { get; set; } = 0;
    }
}
