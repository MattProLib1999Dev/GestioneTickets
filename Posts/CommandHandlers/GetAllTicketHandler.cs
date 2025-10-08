using GestioneAccounts.BE.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GestioneTickets.DataAccess;
using GestioneTickets.Model;

public class GetAllTicketHandler : IRequestHandler<GetAllTicket, ICollection<Ticket>>
{
    private readonly ApplicationDbContext _context;

    public GetAllTicketHandler(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<ICollection<Ticket>> Handle(GetAllTicket request, CancellationToken cancellationToken)
    {
        var tickets = await _context.Tickets.ToListAsync(cancellationToken);
        return tickets;
    }
}
