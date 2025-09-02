using GestioneAccounts.DataAccess;
using MediatR;

namespace GestioneAccounts.Posts.Queries
{
	public class GetValoriById: IRequest<Valori>
	{
		public long? Id { get; set; }
	}
}