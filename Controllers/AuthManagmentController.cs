using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Configuration;
using System.Text.Json;
using System.Text;
using AutoMapper;
using MediatR;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GestioneTickets.DTOs;
using GestioneTickets.DataAccess;
namespace GestioneTickets.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagmentController : ControllerBase
    {
        private readonly ILogger<AuthManagmentController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _account;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IMapper _mapper;


        public AuthManagmentController(
            ILogger<AuthManagmentController> logger,
            ApplicationDbContext context,
            UserManager<Account> account,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _account = account;
            _mapper = mapper;
            _jwtConfig = optionsMonitor.CurrentValue;
            _userManager = userManager;
        }

        [HttpPost("create")]
[AllowAnonymous]
[ProducesResponseType(typeof(Account), 200)]
public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto, [FromServices] IHttpClientFactory httpClientFactory)
{
    var account = new Account
    {
        Nome = dto.Nome,
        Voce = dto.Voce,
        DataCreazione = dto.DataCreazione,
        OreLavorate = dto.OreLavorate,
        UserName = dto.Nome,
        Email = dto.Email
    };

    var result = await _account.CreateAsync(account, dto.Password);

    if (!result.Succeeded)
        return BadRequest(result.Errors);

    // ----> In parallelo: chiamo anche l’API Register (fire-and-forget)
    var client = httpClientFactory.CreateClient();

    var registerModel = new GetAccountDto
    {
        Email = dto.Email,
        Password = dto.Password
    };

    var json = JsonSerializer.Serialize(registerModel);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // Chiamata parallela senza bloccare la risposta principale
    _ = Task.Run(async () =>
    {
        try
        {
            var response = await client.PostAsync("https://localhost:5001/api/Account/register", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Chiamata parallela a Register fallita: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella chiamata parallela a Register");
        }
    });

    var accountDto = _mapper.Map<CreateAccountDto>(account);
    return Ok(accountDto);
}


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TicketsRegistrationRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _account.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            var isPasswordValid = await _account.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            // Optional: Include user roles or claims if needed
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
