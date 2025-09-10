using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
/*  */
using System.Text;
using System.Text.Json.Serialization;
using GestioneTickets.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GestioneTickets.Configuration;
using GestioneTickets.DataAccess.Repositories;
using GestioneTickets.Abstractions;
using GestioneAccounts.Abstractions;
using GestioneAccounts.Repositories;
using GestioneAccounts.BE.Domain.Models;




var builder = WebApplication.CreateBuilder(args);

// 1. Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. JWT Config
builder.Services.Configure<JwtConfig>(
    builder.Configuration.GetSection("JwtConfig"));

// 5. MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// 6. AutoMapper
builder.Services.AddAutoMapper(typeof(Program)); 

// 7. Authentication JWT
var secret = builder.Configuration.GetValue<string>("JwtConfig:Secret")
    ?? throw new InvalidOperationException("JwtConfig:Secret is not configured.");

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.SaveToken = true;
});

// 8. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 9. Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 3. MediatR
// Registra tutti gli handler nel progetto corrente (o assembly specifici)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    // oppure se i tuoi handler stanno in un altro assembly:
    // cfg.RegisterServicesFromAssembly(typeof(TuoHandler).Assembly);
});

// 10. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestione Tickets API", Version = "v1" });
});
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();







var app = builder.Build();

// === Middleware pipeline ===

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestione Tickets API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication(); // prima dell'autorizzazione
app.UseAuthorization();

app.MapControllers();

app.Run();
