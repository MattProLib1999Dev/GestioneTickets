using Microsoft.EntityFrameworkCore;
using GestioneAccounts.Abstractions;
using GestioneTickets.Model;
using GestioneTickets.DataAccess;
namespace GestioneAccounts.Repositories


{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RoleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Role> CreateRole(Role role)
        {
            _applicationDbContext.Add(role);
            await _applicationDbContext.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteValori(string roleId)
        {
            var role = await _applicationDbContext.Role.FirstOrDefaultAsync(a => a.Id.ToString() == roleId);
            if (role == null)
                return false;

            _applicationDbContext.Role.Remove(role);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Role> GetRoleById(string roleId)
        {
            var role = await _applicationDbContext.Role
                .Include(r => r.Tickets) // se vuoi includere gli accounts associati
                .FirstOrDefaultAsync(r => r.Id.ToString() == roleId);

            if (role == null)
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");

            return role;
        }

        public async Task<ICollection<Role>> GetAllRoles()
        {
            return await _applicationDbContext.Role
                .Include(r => r.Tickets) // se vuoi includere gli accounts associati
                .ToListAsync();
        }

        public async Task<Role> UpdateRole(Role role, int roleId)
        {
            var modifiedRole = await _applicationDbContext.Role.FirstOrDefaultAsync(r => r.Id == roleId);

            if (modifiedRole == null || role == null)
            {
                var nuovoRole = new Role
                {
                    Tickets = role!.Tickets,
                    Id = roleId
                };
                _applicationDbContext.Role.Add(nuovoRole);
                modifiedRole = nuovoRole;
            }
            else
            {
                // aggiorna campi necessari di modifiedRole con quelli di role
                // es: modifiedRole.Name = role.Name; (dipende da proprietà Role)
            }

            await _applicationDbContext.SaveChangesAsync();

            return modifiedRole;
        }

        public async Task<bool> DeleteRole(string roleId)
        {
            var role = await _applicationDbContext.Role.FirstOrDefaultAsync(a => a.Id.ToString() == roleId);
            if (role == null)
                return false;

            _applicationDbContext.Role.Remove(role);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
