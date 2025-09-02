using GestioneAccounts.DataAccess;
using MediatR;

namespace GestioneAccounts.Posts.Commands
{
	public class UpdateValori: IRequest<Valori>
	{
        public string? Nome { get; set; } = String.Empty;
	}
}