using System.Text.Json.Serialization;
using GestioneTickets.Model;

public class RoleDto
{
    public string Roles { get; set; } = string.Empty;
    public int Id = 0;
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Account> Accounts { get; set; } = new List<Account>();



}
