using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestioneTickets.Model;

public class GetAccountDto
{
  public string AccountId { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public Role Role { get; set; } = new Role();
  public string Nome { get; set; } = string.Empty;

}
