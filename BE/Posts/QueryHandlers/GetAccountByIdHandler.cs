using GestioneTickets.Abstractions;
using MediatR;
using GestioneTickets.Model;

namespace GestioneTickets.Posts.QueryHandlers
{
    public class GetAccountByIdHandler(IAccountRepository accountRepository): IRequestHandler<AccountById, GetAccountById>
    {
        public readonly IAccountRepository _accountRepository = accountRepository;
    
        public async Task<GetAccountById> Handle(AccountById request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountById(request.AccountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            var result = new GetAccountById
            {
                Nome = account.Nome,
                Role = new Role
                {
                    Name = account.Nome ?? string.Empty
                }
            };

            return result;
        }
    }
}
