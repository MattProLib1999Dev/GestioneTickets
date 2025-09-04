using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GestioneTickets.Model;

public class CreateTicketDto
{
    public string Titolo { get; set; } = string.Empty;

    public string Descrizione { get; set; } = string.Empty;
    public Categoria Categoria { get; set; } = Categoria.None;

    [JsonIgnore]
    public Role? Role { get; set; } = null;
    public string Nome { get; set; } = string.Empty;
    public string Voce { get; set; } = string.Empty;
    public string ValoreString { get; set; } = string.Empty;

    public DateTime DataCreazione { get; set; } = DateTime.Now;

    public DateTime DataChiusura { get; set; } = DateTime.Now;

    public bool Canc { get; set; } = false;

    public int ID_ticket{ get; set; } = 0;

}
