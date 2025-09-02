using MediatR;
using GestioneAccounts.BE.Domain.Models;
using System.Collections.Generic;

public class OrderAccounts : IRequest<List<Account>>
{
    public string? Name { get; set; }
}
