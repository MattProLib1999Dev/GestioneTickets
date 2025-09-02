using GestioneAccounts.BE.Domain.Models;
using MediatR;

public class PaginateItemsQuery : IRequest<List<Account>>
{
    public int Page { get; set; }
    public int PageSize { get; set; } = 5;
}
