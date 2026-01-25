using Microsoft.AspNetCore.Mvc;
using GestioneTickets.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using GestioneTickets.Model;
using GestioneAccounts.Posts.Commands;
using GestioneTickets.Repositories;
using GestioneTickets.Abstractions;


namespace GestioneTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<TicketController> _logger;
        public readonly TicketRepository accountRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public readonly ITicketRepository _ticketRepository;


        public TicketController(
        ILogger<TicketController> logger,
        ApplicationDbContext context,
        IMediator mediator,
        IWebHostEnvironment env,
        IMapper mapper,
        ITicketRepository ticketRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _env = env;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateTicketDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            if (dto == null)
                return BadRequest("Ticket data is required.");

            // Verifica che l'Account esista
            var account = await _context.Users.FindAsync(dto.AccountId);
            if (account == null)
                return BadRequest("Account non trovato.");

            // Crea nuovo ticket
            var ticketObj = new Ticket
            {
                Nome = dto.Nome,
                Descrizione = dto.Descrizione,
                Email = dto.Email,
                Categoria = dto.Categoria,
                DataCreazione = dto.DataCreazione,
                DataChiusura = dto.DataChiusura,
                AccountId = dto.AccountId
            };

            try
            {
                _context.Tickets.Add(ticketObj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Gestione errori FK
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_Tickets_AspNetUsers_AccountId"))
                {
                    return BadRequest("Impossibile creare il ticket: Account non valido.");
                }
                throw; // Altrimenti rilancia
            }

            // Mappatura con AutoMapper
            var createdTicketDto = _mapper.Map<CreateTicketDto>(ticketObj);
            return Ok(createdTicketDto);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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
        // GET: api/Account/all con mapper
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var ticket = await _context.Tickets
                    .ToListAsync();

                if (ticket == null || !ticket.Any())
                {
                    return NotFound(new { message = "Nessun account trovato." });
                }

                var accountDtos = ticket.Select(ticket =>
                {

                    return new GetTicketDtoInput
                    {
                        Nome = ticket.Nome,
                        Descrizione = ticket.Descrizione,
                        Email = ticket.Email,
                        Categoria = ticket.Categoria,
                        DataChiusura = ticket.DataChiusura,
                        DataCreazione = ticket.DataCreazione,
                        AccountId = ticket.AccountId
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
            var account = await _context.Tickets.FirstOrDefaultAsync(a => a.AccountId == CreateAccountDto.AccountId);
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
