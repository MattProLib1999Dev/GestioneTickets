using Microsoft.AspNetCore.Mvc;
using GestioneTickets.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.Posts.Queries;
using GestioneAccounts.Posts.Commands;
using GestioneTickets.DataAccess.Repositories;


namespace GestioneAccounts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;
        public readonly TicketRepository accountRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly UserManager<Ticket> _userManager;



        public AccountController(
        ILogger<AccountController> logger,
        ApplicationDbContext context,
        IMediator mediator,
        IWebHostEnvironment env,
        IMapper mapper,
        UserManager<Ticket> userManager) // ✅ Assicurati che sia passato qui
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _env = env;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); // ✅
        }

        // POST: api/Account/create
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Ticket), 200)]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Ticket data is required.");
            }

            // Mappa DTO a Entity
            var existingTicket = await _userManager.FindByNameAsync(dto.Titolo);
            if (existingTicket != null)
            {
                return Conflict(new { message = "Titolo già in uso." });
            }

            existingTicket = await _userManager.FindByNameAsync(dto.Descrizione);
            if (existingTicket != null)
            {
                return Conflict(new { message = "Email già in uso." });
            }

            // Crea l'account
            var ticket = await _userManager.FindByIdAsync(dto.ID_ticket.ToString());
            if (ticket != null)
            {
                return Conflict(new { message = "Id ticket già in uso." });
            }
            {
                var ticketObj = new Ticket
                {
                    AccessFailedCount = 0,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Email = dto.Descrizione,
                    EmailConfirmed = false,
                    LockoutEnabled = false,
                    LockoutEnd = null,
                    NormalizedEmail = dto.Descrizione.ToUpper(),
                    NormalizedUserName = dto.Titolo.ToUpper(),
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    Titolo = dto.Titolo,
                    Descrizione = dto.Descrizione,
                    Categoria = dto.Categoria,
                    Role = dto.Role,
                    Nome = dto.Nome,
                    ValoreString = dto.ValoreString,
                    DataCreazione = dto.DataCreazione,
                    DataChiusura = dto.DataChiusura,
                    Canc = dto.Canc,
                    ID_utente = dto.ID_ticket

                };

                // Usa UserManager per gestire la creazione
                var result = await _userManager.CreateAsync(ticketObj);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var accountDto = _mapper.Map<CreateTicketDto>(ticketObj);
                return Ok(accountDto);
            }


        }

        // GET: api/Account/{id}
            [HttpGet("{id}")]
            public async Task<IActionResult> GetAccountById(int id)
            {
                var query = new GetTicketById { Id = id };
                var account = await _mediator.Send(query);

                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                return Ok(account);
            }

            // PUT: api/Account/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateTicket command)
            {
                if (command == null)
                {
                    return BadRequest("Account data is required.");
                }

            command.Id = Guid.Empty;
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
                var deleteAccountCommand = new DeleteTicket { Id = id };
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
                var exists = await _context.Tickets.AnyAsync(a => a.Nome == nome);

                if (!exists)
                {
                    return NotFound(new { message = "Nessun account trovato con questo nome." });
                }

                // ✅ Se esiste, prosegui con MediatR
                var query = new SearchTicket { Nome = nome };
                var result = await _mediator.Send(query);

                return Ok(result);
            }

            // GET: api/Account/orderByName
            [HttpGet("orderByName")]
            public async Task<IActionResult> OrderByName()
            {
                var accounts = await _context.Tickets
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
                    var accounts = await _context.Tickets
                        .Include(a => a.Role) // Include corretto su collection di ruoli
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
                        var ruoloNomi = account.Role != null
                            ? new List<string> { account.Role.Name }
                            : new List<string>();

                        return new GetAccountDto
                        {
                            Id = idGuid.ToString(),
                            Email = account.Email,
                            UserName = account.UserName,
                            Roles = ruoloNomi.ToString()// supponendo che GetAccountDto abbia questa proprietà
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
            public async Task<IActionResult> ApprovaOreLavorate([FromBody] CreateTicketDto CreateAccountDto)
            {
                if (CreateAccountDto == null)
                {
                    return BadRequest("Account data is required.");
                }

                // Trova l'account esistente
                var account = await _context.Tickets.FirstOrDefaultAsync(a => a.ID_utente == CreateAccountDto.ID_ticket);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                // Aggiorna le ore lavorate
                var accountDto = _mapper.Map<CreateTicketDto>(account);

                // Salva le modifiche nel database
                _context.Tickets.Update(account);
                await _context.SaveChangesAsync();

                return Ok(accountDto);
            }

        }

    }
