using MediatR;

namespace GestioneAccounts.Posts.Commands
{
    public class DeleteTicket : IRequest<Unit>
    {
        public long Id { get; internal set; }
    }
}
