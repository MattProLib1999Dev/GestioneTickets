using GestioneTickets.Model;
using MediatR;
using GestioneTickets.DataAccess;
public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Role>
{
    private readonly ApplicationDbContext _context;

    public CreateRoleHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            AccountId = request.AccountId,
            Name = request.Name,
            Roles = request.Roles
        };

        _context.Role.Add(role);
        await _context.SaveChangesAsync();

        return role;
    }
}
