using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.DataAccess;
using GestioneAccounts.BE.Domain.Models;

namespace GestioneAccounts.Posts.Queries
{
    public class GetAllAccountsQuery : IRequest<ICollection<Ticket>>
    {
    }

    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, ICollection<Ticket>>
    {
      private readonly ApplicationDbContext _context;

      public GetAllAccountsHandler(ApplicationDbContext context)
      {
          _context = context;
      }

      public async Task<ICollection<Ticket>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
      {
          return await _context.Tickets.ToListAsync(cancellationToken);
      }
    }
}

