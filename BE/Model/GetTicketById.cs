using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using GestioneTickets.Model;
using System.ComponentModel;
using Microsoft.Identity.Client;

namespace GestioneTickets.Model
{
    public class GetTicketById : IdentityUser<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public int AccountId { get; set; } = 0;

        [StringLength(30), MinLength(5)]
        public string Titolo { get; set; } = string.Empty;

        [StringLength(100), MinLength(2)]
        public string Descrizione { get; set; } = string.Empty;
        [StringLength(20), MinLength(2)]
        public Categoria Categoria { get; set; } = Categoria.None;
        public Role? Role { get; set; } = null;
        [StringLength(100), MinLength(2)]
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime? DataCreazione { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataChiusura { get; set; }

        [StringLength(1), MinLength(1)]
        public bool Canc { get; set; } = false;

        public int ID_utente { get; set; } = 0;

        [StringLength(100), MinLength(2), PasswordPropertyText(true)]
        public string Password { get; set; } = string.Empty;
        public Account Account { get; set; } = new Account();


    }
}
