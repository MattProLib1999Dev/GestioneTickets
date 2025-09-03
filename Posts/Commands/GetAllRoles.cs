using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;
using MediatR;
using System;
using System.Collections.Generic;

public class GetAllRoles : IRequest<List<Role>>
{
    public Guid AccountId { get; set; } = Guid.Empty;
    public string Roles { get; set; } = string.Empty;
}
