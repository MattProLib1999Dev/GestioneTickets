using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.DataAccess;
using GestioneAccounts.BE.Domain.Models; // Ensure this namespace is correct for 'Account'
public class PaginateItemsHandler : IRequestHandler<PaginateItemsQuery, List<Ticket>>
{
    private readonly ApplicationDbContext _context;

    public PaginateItemsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> Handle(PaginateItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tickets // Assuming 'Accounts' is the correct DbSet for 'Account'
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
    }
}
