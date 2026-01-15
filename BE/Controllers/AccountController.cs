using Microsoft.AspNetCore.Mvc;
using MediatR;
using GestioneTickets.Model;
using GestioneTickets.Repositories;
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
        [HttpGet("id")]
        public async Task<IActionResult> GetAccountById([FromQuery] int id)
        {
           try
            {
                var accounts = await _context.Account
                    .ToListAsync();

                if (accounts == null || !accounts.Any())
                {
                    return NotFound(new { message = "Nessun account trovato." });
                }

                var accountDtos = accounts.Select(account =>
                {
    
                    return new Account
                    {
                        Nome = account.Nome,
                        Cognome = account.Cognome,
                        Email = account.Email,
                        DataCreazione = account.DataCreazione,
                        DataChiusura = account.DataChiusura,
                        OreLavorate = account.OreLavorate,
                    };
                });

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

        // PUT: api/Account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account command)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleteAccountCommand = new Account { Id = id };
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

            var query = new SearchAccount
            {
                Nome = nome
            };
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

        /* [HttpPost("upload")]
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
        } */

        [HttpGet("all")]
        // GET: api/Account/all con mapper
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _context.Account
                    .ToListAsync();

                if (accounts == null || !accounts.Any())
                {
                    return NotFound(new { message = "Nessun account trovato." });
                }

                var accountDtos = accounts.Select(account =>
                {
    
                    return new Account
                    {
                        Nome = account.Nome,
                        Cognome = account.Cognome,
                        Email = account.Email,
                        DataCreazione = account.DataCreazione,
                        DataChiusura = account.DataChiusura,
                        OreLavorate = account.OreLavorate,
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
            var account = await _context.Account.FirstOrDefaultAsync(a => a.Password == CreateAccountDto.Password);
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

        // gestisci in questo metodo l'assegnazione del ruolo di default "User" e nello stesso tempo dagli la possibilità di essere 'Admin'
        [HttpPost("manageRole")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAndManageRole([FromBody] CreateAccountDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Email is already registered." });
            }

            var account = new Account
            {
                Email = model.Email,
                Nome = model.Nome,
                Cognome = model.Cognome,
                DataCreazione = model.DataCreazione,
                OreLavorate = model.OreLavorate,
                UserName = model.Email // Imposta UserName uguale a Email
            };

            var result = await _userManager.CreateAsync(account, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Message = "User creation failed.", Errors = errors });
            }

            await _userManager.AddToRoleAsync(account, "User");

            if (model.IsAdmin)
            {
                await _userManager.AddToRoleAsync(account, "Admin");
            }

            var accountDto = _mapper.Map<CreateAccountDto>(account);
            return Ok(accountDto);

        }
    }
}