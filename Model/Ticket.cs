using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Categoria? Categoria { get; set; }
    public DateTime DataCreazione { get; set; } = DateTime.Today;
    public DateTime? DataChiusura { get; set; }

    public int AccountId { get; set; } // FK verso Account
    public Account Account { get; set; } = null!;
}
