using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestioneTickets.Model;

public class GetAccountDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int AccountId { get; set; } = 0;

    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
    public DateTime? DataChiusura { get; set; }
    public double OreLavorate { get; set; } = 0.0;

}
