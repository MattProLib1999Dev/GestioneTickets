using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneAccounts.DataAccess;
using GestioneAccounts.BE.Domain.Models;

public class OrderAccountsQueryHandler(ApplicationDbContext context) : IRequestHandler<OrderAccounts, List<Account>>
{
    private readonly ApplicationDbContext _context = context;

  public async Task<List<Account>> Handle(OrderAccounts request, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .Where(a => !string.IsNullOrEmpty(a.Nome))
            .OrderBy(a => a.Nome)
            .ToListAsync(cancellationToken);
    }
}
