using System.Collections;
using Microsoft.AspNetCore.Identity;

public class Role: IdentityRole<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Ticket Ticket { get; set; } = null!;
    public ICollection<Account> Account { get; set; } = new List<Account>();
}