using MediatR;

namespace GestioneAccounts.Posts.Commands
{
    public class DeleteAccount : IRequest<Unit>
    {
        public long Id { get; internal set; }
    }
}
