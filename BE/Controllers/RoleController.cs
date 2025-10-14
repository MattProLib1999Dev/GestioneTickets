using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using GestioneTickets.DataAccess;
using System.Collections.Generic;


namespace GestioneTickets.Controllers
{

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<RoleController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;


    public RoleController(
        ILogger<RoleController> logger,
        ApplicationDbContext context,
        IMediator mediator,
        IWebHostEnvironment env,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _env = env;
        _mapper = mapper;
    }

    // POST: api/Role/create
    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
    {
        if (roleDto == null || roleDto.Roles == "" || string.IsNullOrWhiteSpace(roleDto.Roles))
            return BadRequest(new { message = "Invalid role data." });

        try
        {
                var command = new CreateRoleCommand
                {
                    Id = roleDto.Id,
                    Roles = roleDto.Roles,
                    Name = roleDto.Name
                };

            var createdRole = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetAllRoles), new { id = createdRole.Id }, createdRole);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }


    // GET: api/Role/isAdmin?accountId=123
    [HttpGet("isAdmin")]
    public async Task<IActionResult> IsAdmin([FromQuery] int accountId)
    {
        if (accountId == null)
            return BadRequest(new { message = "invalid account, account ID is required." });

        try
        {
            var account = await _context.Tickets
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
                return NotFound(new { message = "Account not found." });

            else
                return Ok(new { message = "User is an admin." });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking admin status");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    // GET: api/Role/isUser?accountId=123
    [HttpGet("isUser")]
    public async Task<IActionResult> IsUser([FromQuery] int accountId)
    {
        if (accountId == null)
            return BadRequest(new { message = "invalid account, account ID is required." });

        try
        {
            var account = await _context.Tickets
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
                return NotFound(new { message = "Account not found." });

           else
                return Ok(new { message = "User is a standard user." });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user status");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("all-roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _context.Roles
              .Select(role => new Role
              {
                  Account = role.Account,
                  Id = role.Id,
                  Name = role.Name
              })
              .ToListAsync();
            if (roles == null || !roles.Any())
            {
                return NotFound(new { message = "Nessun ruolo trovato." });
            }

            var roleDtos = roles.Select(role =>
            {
                if (role.Account == null)
                {
                    _logger.LogWarning($"AccountId non valido come GUID per il ruolo: {role.Name}");
                }

                return new RoleDto
                {
                    Name = role.Name,
                    Accounts = role.Account
                };
            });

            return Ok(roleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei ruoli: {Message}", ex.Message);

            return StatusCode(500, new
            {
                message = "Errore interno del server",
                error = ex.Message,
                inner = ex.InnerException?.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    //get: api/Role/123
    [HttpGet("rolesById/{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        try
        {
            var role = await _context.Roles
                .Include(r => r.Ticket)  // Include proprietà di navigazione corretta
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);

            if (role == null)
                return NotFound(new { message = "Ruolo non trovato." });

            return Ok(_mapper.Map<RoleDto>(role));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del ruolo con ID: {Id}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }


}
}