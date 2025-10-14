using Microsoft.AspNetCore.Identity;

public class GetTicketDtoOutput
{
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Categoria? Categoria { get; set; }
    public DateTime DataCreazione { get; set; } = DateTime.Today;
    public DateTime DataChiusura { get; set; }
    public int AccountId { get; set; }
   
}