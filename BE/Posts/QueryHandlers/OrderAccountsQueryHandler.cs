using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.Model;
using GestioneTickets.DataAccess;
using GestioneAccounts.BE.Domain.Models;

public class OrderAccountsQueryHandler(ApplicationDbContext context) : IRequestHandler<OrderTicket, List<Ticket>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Ticket>> Handle(OrderTicket request, CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .OrderBy(t => t.Categoria)
            .OrderBy(t => t.DataCreazione)
            .ToListAsync(cancellationToken);
    }
}
