using GestioneAccounts.BE.Domain.Models;
using MediatR;

namespace GestioneAccounts.Posts.Queries
{
	public class GetTicketById: IRequest<Ticket>
	{
        public long Id { get; set; } = 0;
    }
}
