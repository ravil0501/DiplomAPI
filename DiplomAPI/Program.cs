using DiplomAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Проверим, задана ли строка подключения к базе данных
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Используем базу данных в памяти, если строка подключения не задана
    builder.Services.AddDbContext<DiplomContext>(options =>
        options.UseInMemoryDatabase("InMemoryDatabase"));
}
else
{
    // Используем реальную базу данных, если строка подключения задана
    builder.Services.AddDbContext<DiplomContext>(options =>
        options.UseSqlServer(connectionString));
}

// Добавляем сервисы в контейнер
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация HTTP-конвейера запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DiplomContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();