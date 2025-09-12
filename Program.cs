using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using GestioneTickets.DataAccess;
using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Configuration;
using System.Text;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using GestioneTickets.DataAccess.Repositories;
using GestioneTickets.Abstractions;
using GestioneAccounts.Abstractions;
using GestioneAccounts.DataAccess.Repositories;
using GestioneTickets.Model;

var builder = WebApplication.CreateBuilder(args);

// Forza TLS 1.2 (opzionale per sicurezza aziendale)
AppContext.SetSwitch("System.Net.Security.UseLegacySslProtocols", false);
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

// 1️⃣ DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2️⃣ Identity con Account e Role personalizzati
builder.Services.AddIdentity<Account, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3️⃣ JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var secret = builder.Configuration["JwtConfig:Secret"]!;
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.SaveToken = true;
});

// 4️⃣ MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// 5️⃣ Repository DI
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// 6️⃣ Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 7️⃣ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestione Tickets API", Version = "v1" });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
