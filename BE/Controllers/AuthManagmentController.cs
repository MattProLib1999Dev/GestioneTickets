using AutoMapper;
using GestioneTickets.Abstractions;
using GestioneTickets.Configuration;
using GestioneTickets.DTOs;
using GestioneTickets.Model;
using GestioneTickets.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace GestioneTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagmentController : ControllerBase
    {
        private readonly ILogger<AuthManagmentController> _logger;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAccountRepository _accountRepository;
        private readonly JwtConfig _jwtConfig;
        private readonly IMapper _mapper;

        public AuthManagmentController(
    ILogger<AuthManagmentController> logger,
    UserManager<Account> userManager,
    RoleManager<Role> roleManager,  // <- aggiungi
    IAccountRepository accountRepository,
    IOptionsMonitor<JwtConfig> optionsMonitor,
    IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;      // <- assegna
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] GetAllAccountDto registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registration == null)
                return BadRequest("Dati di registrazione non validi.");

            _logger.LogInformation($"Registration attempt for {registration.Email}");

            // Controlla se l'email esiste già
            var emailExist = await _userManager.FindByEmailAsync(registration.Email);
            if (emailExist != null)
                return BadRequest("Email già in uso.");

            // Creazione dell'utente senza impostare direttamente la password
            var account = new Account
            {
                UserName = registration.Email,
                Email = registration.Email,
                Nome = registration.Nome,
                Cognome = registration.Cognome,
                DataCreazione = DateTime.UtcNow
            };

            // Passa la password a UserManager
            var result = await _userManager.CreateAsync(account, registration.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Controlla se il ruolo "User" esiste e, se non c'è, lo crea
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new Role { Name = "User", NormalizedName = "USER" });
            }

            // Assegna il ruolo "User" all'utente appena creato
            await _userManager.AddToRoleAsync(account, "User");

            return Ok(new { Message = "Utente registrato con successo." });
        }





        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TicketsRegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Email = user.Email,
                Token = token
            });
        }

        private string GenerateJwtToken(Account account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", account.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

    }
}
