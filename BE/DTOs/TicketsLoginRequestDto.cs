using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace GestioneTickets.DTOs;

public class TicketsLoginRequestDto
{
  [Required(ErrorMessage = "Email is required.")]
  [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Password is required.")]
  [StringLength(50, ErrorMessage = "Password cannot be longer than 50 characters.")]
  public string Password { get; set; } = string.Empty;

}
