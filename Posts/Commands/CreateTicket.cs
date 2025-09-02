using System.ComponentModel.DataAnnotations;
using GestioneAccounts.BE.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using GestioneTickets.Model;

namespace GestioneAccounts.Posts.Commands
{
    public class CreateTicket : IRequest<Ticket>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(30), MinLength(5)]
        public string Titolo { get; set; } = string.Empty;

        [StringLength(100), MinLength(2)]
        public string Descrizione { get; set; } = string.Empty;
        [StringLength(20), MinLength(2)]
        public Categoria Categoria { get; set; } = Categoria.None;
        public Role? Role { get; set; } = null;
        [StringLength(100), MinLength(2)]
        public string Nome { get; set; } = string.Empty;
        public string ValoreString { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime DataCreazione { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime DataChiusura { get; set; } = DateTime.Now;

        [StringLength(1), MinLength(1)]
        public bool Canc { get; set; } = false;

        public int ID_utente { get; set; } = 0;

    }
}
