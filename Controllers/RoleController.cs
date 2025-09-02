using Microsoft.AspNetCore.Mvc;
using GestioneAccounts.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using GestioneAccounts.BE.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper.Internal;
using GestioneTickets.DataAccess;
using GestioneAccounts.Posts.Commands;

namespace GestioneAccounts.Controllers;

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
    if (roleDto == null || roleDto.AccountId == Guid.Empty)
      return BadRequest(new { message = "Invalid role data." });

    try
    {
      var command = new CreateRoleCommand
      {
        AccountId = roleDto.AccountId,
        Roles = roleDto.Roles,
        Name = "New Role"
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
  public async Task<IActionResult> IsAdmin([FromQuery] Guid accountId)
  {
    if (accountId == Guid.Empty)
      return BadRequest(new { message = "Valid account ID is required." });

    try
    {
      var account = await _context.Tickets
          .Include(a => a.Role)
          .FirstOrDefaultAsync(a => a.Id == accountId);

      if (account == null)
        return NotFound(new { message = "Account not found." });

      if (account.Role != null)
        return Ok(new { message = "User is an admin." });

      return Forbid("User is not an admin.");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error checking admin status");
      return StatusCode(500, new { message = "Internal server error" });
    }
  }

  // GET: api/Role/isUser?accountId=123
  [HttpGet("isUser")]
  public async Task<IActionResult> IsUser([FromQuery] Guid accountId)
  {
    if (accountId == Guid.Empty)
      return BadRequest(new { message = "Valid account ID is required." });

    try
    {
      var account = await _context.Tickets
          .Include(a => a.Role)
          .FirstOrDefaultAsync(a => a.Id == accountId);

      if (account == null)
        return NotFound(new { message = "Account not found." });

      if (account.Role != null)
        return Ok(new { message = "User is a standard user." });

      return Forbid("User is not a standard user.");
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
      var roles = await _context.Role
        .Select(role => new
        {
          AccountId = role.AccountId,
          Roles = role.Roles
        })
        .ToListAsync();
      if (roles == null || roles.Count == 0)
        {
            return NotFound(new { message = "Nessun ruolo trovato." });
        }

      var roleDtos = roles.Select(role =>
      {
        Guid accountGuid;
        if (!Guid.TryParse(role.AccountId.ToString(), out accountGuid))
        {
          _logger.LogWarning($"AccountId non valido come GUID per il ruolo: {role.Roles}");
          accountGuid = Guid.Empty;
        }

        return new RoleDto
        {
          AccountId = accountGuid,
          Roles = role.Roles,
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
        var role = await _context.Role
            .Include(r => r.Tickets)  // Include proprietà di navigazione corretta
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
