using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;

public class CreateAccountDto
{
        public int IdAccount { get; set; } = 0;

        public string Nome { get; set; } = string.Empty;
        public string Voce { get; set; } = string.Empty;
        public DateTime DataCreazione { get; set; } = DateTime.Now;
        public double OreLavorate { get; set; } = 0.0;
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

}