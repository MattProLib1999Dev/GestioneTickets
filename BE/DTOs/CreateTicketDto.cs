public class CreateTicketDto
{
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Categoria? Categoria { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime DataChiusura { get; set; }
    public int AccountId { get; set; } // Nullable
}
