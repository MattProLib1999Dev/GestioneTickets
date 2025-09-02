using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllValoriHandler(ApplicationDbContext context) : IRequestHandler<GetAllValori, ICollection<Account>>
{
    private readonly ApplicationDbContext _context = context;

  public async Task<ICollection<Account>> Handle(GetAllValori request, CancellationToken cancellationToken)
    {
        // Restituisce tutti i valori dal database
        return await _context.Accounts.ToListAsync(cancellationToken);
    }
}
