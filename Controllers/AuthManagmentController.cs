using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using GestioneTickets.Configuration;
using System.Text;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GestioneTickets.DataAccess;
using GestioneTickets.DTOs;
using GestioneAccounts.BE.Domain.Models;
using GestioneAccounts.DataAccess.Repositories;
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
        public async Task<IActionResult> CreateAccountWithRegister([FromBody] CreateAccountDto dto, [FromServices] AccountRepository accountRepository)
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

            await accountRepository.CreateAccount(account);
            var accountDto = _mapper.Map<CreaAccountDtoOutput>(account);
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
