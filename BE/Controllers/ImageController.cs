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
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<ImageController> _logger;
        public readonly AccountRepository accountRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;



        public ImageController(
        ILogger<ImageController> logger,
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
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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

        
    }
}