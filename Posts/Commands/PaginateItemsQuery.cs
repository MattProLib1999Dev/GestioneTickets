using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class PaginateItemsQuery : IRequest<List<Ticket>>
{
    public int Page { get; set; }
    public int PageSize { get; set; } = 5;
}
