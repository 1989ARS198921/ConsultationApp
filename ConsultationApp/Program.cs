using Microsoft.EntityFrameworkCore;
using ConsultationApp.Data;
using ConsultationApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключаем SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрируем все сервисы
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IConsultantService, ConsultantService>();

// Добавляем контроллеры
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();