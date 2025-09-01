
using GestioneTickets.Model;
namespace GestioneAccounts.Abstractions;
public interface IRoleRepository
{
    Task<ICollection<Role>> GetAllRoles();
    Task<Role> GetRoleById(string roleId);
    Task<Role> CreateRole(Role role);
    Task<Role> UpdateRole(Role role, int roleId);
    Task<bool> DeleteRole(string roleId);
}
