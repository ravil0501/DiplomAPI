using DiplomAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��������, ������ �� ������ ����������� � ���� ������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // ���������� ���� ������ � ������, ���� ������ ����������� �� ������
    builder.Services.AddDbContext<DiplomContext>(options =>
        options.UseInMemoryDatabase("InMemoryDatabase"));
}
else
{
    // ���������� �������� ���� ������, ���� ������ ����������� ������
    builder.Services.AddDbContext<DiplomContext>(options =>
        options.UseSqlServer(connectionString));
}

// ��������� ������� � ���������
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ������������ HTTP-��������� ��������
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