using GestioneAccounts.DataAccess;
using GestioneAccounts.BE.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestioneAccounts.Posts.Queries
{
    public class GetAllAccountsQuery : IRequest<ICollection<Account>>
    {
    }

    public class GetAllAccountsHandler(ApplicationDbContext context) : IRequestHandler<GetAllAccountsQuery, ICollection<Account>>
    {
      private readonly ApplicationDbContext _context = context;

    public async Task<ICollection<Account>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
      {
        // Fetch all accounts from the database
        var accounts = await _context.Accounts.ToListAsync(cancellationToken);

        return accounts;
      }
    }
}

