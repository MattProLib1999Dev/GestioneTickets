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
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
            RoleManager<Role> roleManager,
            IAccountRepository accountRepository,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] GetAllAccountDto registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailExist = await _userManager.FindByEmailAsync(registration.Email);
            if (emailExist != null)
                return BadRequest("Email già in uso.");

            var account = new Account
            {
                UserName = registration.Email,
                Email = registration.Email,
                Nome = registration.Nome,
                Cognome = registration.Cognome,
                DataCreazione = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(account, registration.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assegna il ruolo. Assicurati che il ruolo esista già nel DB (vedi passaggio sotto)
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
                return Unauthorized(new { Message = "Email o password non validi." });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized(new { Message = "Email o password non validi." });

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new
            {
                Email = user.Email,
                Token = token
            });
        }

        private string GenerateJwtToken(Account account, IList<string> roles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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