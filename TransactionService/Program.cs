using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.Repositories;
using TransactionService.Services;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Configurar la base de datos (SQL Server)
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Redis
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString; // Cadena de conexión a Redis
    options.InstanceName = "TransactionSystem_"; // Nombre de la instancia en Redis
});

// Inyección de dependencias
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<TransactionService>();

// Registrar el servicio RedisQueueService
builder.Services.AddSingleton<RedisQueueService>();

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar middleware para autorización
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();
