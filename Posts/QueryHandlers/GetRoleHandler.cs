using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.Model;


public class GetAllRolesHandler : IRequestHandler<GetAllTicket, List<Ticket>>
{
    private readonly ApplicationDbContext _context;

    public GetAllRolesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> Handle(GetAllTicket request, CancellationToken cancellationToken)
    {
        return await _context.Tickets.ToListAsync(cancellationToken);
    }
}
