using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GestioneAccounts.BE.Domain.Models;
using Microsoft.AspNetCore.Identity;
namespace GestioneTickets.Model;

public class Role : IdentityRole<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonIgnore]
    public int Id { get; set; }
    public Guid AccountId { get; set; } = Guid.Empty;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();  // ✅
    public string Roles { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;





}
