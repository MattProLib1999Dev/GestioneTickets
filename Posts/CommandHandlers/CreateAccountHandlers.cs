using GestioneAccounts.Abstractions;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Commands;
using MediatR;

namespace GestioneAccounts.Posts.CommandHandlers;
public class CreateAccountHandlers : IRequestHandler<CreateAccount, Account>
{
  private readonly IAccountRepository _accountRepository;

  public CreateAccountHandlers(IAccountRepository accountRepository)
  {
      _accountRepository = accountRepository;
  }

  public async Task<Account> Handle(CreateAccount request, CancellationToken cancellationToken)
  {
      var account = new Account
      {
        Nome = request.Nome,
        ValoreString = request.valoreString,
        Voce = request.voce,
        DataCreazione = DateTime.Now,
        Id = new Guid(),
      };
      return await _accountRepository.CreateAccount(account);
  }
}
