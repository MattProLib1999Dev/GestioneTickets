using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using MediatR;
using System;
using System.Collections.Generic;

public class GetAllTicket : IRequest<List<Ticket>>
{
    public Guid TicketId { get; set; } = Guid.Empty;
    public string Roles { get; set; } = string.Empty;
}
