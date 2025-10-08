using Microsoft.AspNetCore.Identity;

public class Ticket: IdentityUser<int>
{
    public int? Id { get; set; } = 0;
    public int? RoleId { get; set; } = 0;
    public string Nome { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime DataCreazione { get; set; } = DateTime.Today;
    public DateTime DataChiusura { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Role Role { get; set; } = new Role();
}