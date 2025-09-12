using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using GestioneTickets.Model;

namespace GestioneAccounts.BE.Domain.Models
{
    public class Account : IdentityUser<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;

        public int AccountId { get; set; }

        // 🔹 relazione EF mantenuta, ma esclusa dal JSON
        [JsonIgnore, NotMapped]
        public Role? Role { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Voce { get; set; } = string.Empty;
        public string ValoreString { get; set; } = string.Empty;
        public DateTime DataCreazione { get; set; } = DateTime.Now;

        public double OreLavorate { get; set; } = 0.0;

        // 🔹 lista di ruoli gestita da EF, ma non serializzata
        [JsonIgnore, NotMapped]
        public ICollection<Role> Roles { get; set; } = new List<Role>();

        // 🔹 lista di ticket gestita da EF, ma non serializzata
        [JsonIgnore, NotMapped]
        public ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
        public string Password { get; set; } = string.Empty;
    }
}
