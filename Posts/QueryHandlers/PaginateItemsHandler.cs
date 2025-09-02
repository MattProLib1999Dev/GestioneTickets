using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class PaginateItemsHandler(ApplicationDbContext context) : IRequestHandler<PaginateItemsQuery, List<Account>>
{
    private readonly ApplicationDbContext _context = context;

  public async Task<List<Account>> Handle(PaginateItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .OrderBy(i => i.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(i => new Account { Id = i.Id, Nome = i.Nome, Valori = i.Valori, DataCreazione = i.DataCreazione, ValoreString = i.ValoreString, Voce = i.Voce })
            .ToListAsync(cancellationToken);
    }
}
