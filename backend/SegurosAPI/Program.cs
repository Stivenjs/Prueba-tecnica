using Microsoft.EntityFrameworkCore;
using SegurosAPI.Data;
using SegurosAPI.Middleware;
using SegurosAPI.Repositories.Implementations;
using SegurosAPI.Repositories.Interfaces;
using SegurosAPI.Services.Implementations;
using SegurosAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Entity Framework Core con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de Dependencias - Repositories
builder.Services.AddScoped<IAseguradoRepository, AseguradoRepository>();

// Inyección de Dependencias - Services
builder.Services.AddScoped<IAseguradoService, AseguradoService>();

// Configurar CORS para permitir conexión con Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configurar Swagger para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

// Middleware de manejo global de excepciones
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Seguros API v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz (/)
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
