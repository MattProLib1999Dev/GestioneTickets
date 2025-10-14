using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using GestioneTickets.Model;

namespace GestioneTickets.Model
{
    public class GetAllAccountDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public DateTime? DataCreazione { get; set; } = DateTime.Today;
        public DateTime? DataChiusura { get; set; }
        public double OreLavorate { get; set; } = 0.0;
        public string ?Password { get; set; }
        public string Email { get; set; } = string.Empty;

    }
}
