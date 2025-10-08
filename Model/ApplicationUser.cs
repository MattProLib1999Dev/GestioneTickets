using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneAccounts.BE.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [StringLength(100), MinLength(2), Required]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100), MinLength(2), Required]
        public string Cognome { get; set; } = string.Empty;

        [DataType(DataType.Date), Required]
        public DateTime DataNascita { get; set; } = DateTime.Now;

    }
}
