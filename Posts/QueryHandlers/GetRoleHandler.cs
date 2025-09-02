using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAllRolesHandler : IRequestHandler<GetAllRoles, List<Role>>
{
    private readonly ApplicationDbContext _context;

    public GetAllRolesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> Handle(GetAllRoles request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .Where(r => request.Roles.Contains(r.Name))
            .ToListAsync(cancellationToken);
    }
}
