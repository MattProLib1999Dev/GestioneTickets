using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Account : IdentityUser<int>
{
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
    public DateTime? DataChiusura { get; set; }
    public double OreLavorate { get; set; } = 0.0;

    [NotMapped]
    public string? Password { get; set; }

    // Relazione 1:N con Ticket
    public IList<Ticket> Tickets { get; set; } = new List<Ticket>();
}
