using System.Text.Json.Serialization;
using GestioneAccounts.BE.Domain.Models;

public class RoleDto
{
    public Guid AccountId { get; set; }
    public string Roles { get; set; } = string.Empty;
    public List<Guid> RolesId { get; set; } = new List<Guid>();

    [JsonIgnore]
    public ICollection<Ticket> Accounts { get; set; } = new List<Ticket>();



}
