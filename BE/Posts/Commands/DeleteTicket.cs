using MediatR;

namespace GestioneAccounts.Posts.Commands
{
    public class DeleteTicket : IRequest<Unit>
    {
        public int Id { get; internal set; }
    }
}
