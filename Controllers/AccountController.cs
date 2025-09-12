using Microsoft.AspNetCore.Mvc;
using GestioneAccounts.DataAccess;
using MediatR;
using GestioneAccounts.Posts.Queries;
using GestioneAccounts.Posts.Commands;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using GestioneTickets.DataAccess;



namespace GestioneAccounts.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AccountController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<AccountController> _logger;
    public readonly AccountRepository accountRepository;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;
    private readonly UserManager<Account> _userManager;



    public AccountController(
    ILogger<AccountController> logger,
    ApplicationDbContext context,
    IMediator mediator,
    IWebHostEnvironment env,
    IMapper mapper,
    UserManager<Account> userManager) 
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      _env = env;
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); // ✅
    }

    // GET: api/Account/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
      var getAccount = new GetTicketById { Id = id };
      var account = await _mediator.Send(getAccount);

      if (account == null)
      {
        return NotFound();
      }

      return Ok(account);
    }

    // PUT: api/Account/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccount command)
    {
      if (command == null)
      {
        return BadRequest("Account data is required.");
      }

      command.Id = id;
      var updatedAccount = await _mediator.Send(command);

      if (updatedAccount == null)
      {
        return NotFound("Account not found.");
      }

      return Ok(updatedAccount);
    }

    // DELETE: api/Account/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
      var deleteAccountCommand = new DeleteAccount { Id = id };
      var result = await _mediator.Send(deleteAccountCommand);

      if (result != null)
      {
        return Ok(new { message = "Account eliminato con successo" });
      }

      return BadRequest("Account deletion failed.");
    }

    // GET: api/Account/search
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string nome)
    {
      if (string.IsNullOrWhiteSpace(nome))
      {
        return BadRequest(new { message = "Il nome è obbligatorio." });
      }

      // 🔍 Controllo diretto se esiste almeno un account con quel nome
      var exists = await _context.Account.AnyAsync(a => a.Nome == nome);

      if (!exists)
      {
        return NotFound(new { message = "Nessun account trovato con questo nome." });
      }

      var query = new SearchTicket { Nome = nome };
      var result = await _mediator.Send(query);

      return Ok(result);
    }

    // GET: api/Account/orderByName
    [HttpGet("orderByName")]
    public async Task<IActionResult> OrderByName()
    {
      var accounts = await _context.Account
          .OrderBy(a => a.Nome)
          .ToListAsync();
      return Ok(accounts);
    }

    [HttpPost("upload")]
    public IActionResult UploadBase64Image([FromBody] ImageUploadRequest request)
    {
      try
      {
        if (string.IsNullOrEmpty(_env.WebRootPath))
        {
          return StatusCode(500, new { error = "WebRootPath is null. Verifica che wwwroot sia configurata correttamente." });
        }
        Console.WriteLine("WebRootPath = " + _env.WebRootPath);


        var image = Base64Image.Parse(request.Base64Image);

        string extension = image.ContentType switch
        {
          "image/png" => ".png",
          "image/jpeg" => ".jpg",
          "image/gif" => ".gif",
          _ => ".bin"
        };

        string fileName = $"matt{Guid.NewGuid()}{extension}";
        string folderPath = Path.Combine(_env.WebRootPath, "assets", "img");
        Directory.CreateDirectory(folderPath);

        Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, fileName);

        System.IO.File.WriteAllBytes(filePath, image.FileContents);

        return Ok(new { fileName });
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

[HttpGet("all")]
public async Task<IActionResult> GetAllAccounts()
{
    try
    {
        var accounts = await _context.Account
            .Include(a => a.Roles) // Include corretto su collection di ruoli
            .ToListAsync();

        if (accounts == null || !accounts.Any())
        {
            return NotFound(new { message = "Nessun account trovato." });
        }

        var accountDtos = accounts.Select(account =>
        {
            Guid idGuid;
            if (!Guid.TryParse(account.Id.ToString(), out idGuid))
            {
                _logger.LogWarning($"Account ID non valido come GUID: {account.Id}");
                idGuid = Guid.Empty;
            }

            // Mappatura DTO con lista di ruoli (nomi ruoli)
            var ruoloNomi = account.Roles != null
                ? account.Roles.Select(r => r.Name).ToList()
                : new List<string>();

            return new GetAccountDto
            {
                Email = account.Email,
                Password = account.Password,
            };
        }).ToList();

        return Ok(accountDtos);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Errore durante il recupero degli account: {Message}", ex.Message);

        return StatusCode(500, new
        {
            message = "Errore interno del server",
            error = ex.Message,
            inner = ex.InnerException?.Message,
            stackTrace = ex.StackTrace
        });
    }
}

    // POST: account/approvaOreLavorate
    [HttpPost("approvaOreLavorate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApprovaOreLavorate([FromBody] CreateAccountDto CreateAccountDto)
    {
      if (CreateAccountDto == null)
      {
        return BadRequest("Account data is required.");
      }

      // Trova l'account esistente
      var account = await _context.Account.FirstOrDefaultAsync(a => a.Id == CreateAccountDto.IdAccount);
      if (account == null)
      {
        return NotFound("Account not found.");
      }

      // Aggiorna le ore lavorate
      var accountDto = _mapper.Map<CreateAccountDto>(account);

      // Salva le modifiche nel database
      _context.Account.Update(account);
      await _context.SaveChangesAsync();

      return Ok(accountDto);
    }

  }
}
