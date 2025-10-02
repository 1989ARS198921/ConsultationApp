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

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // ← Объявление переменной app

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowReactApp"); // ← Теперь после app.Build()

app.MapControllers();

app.Run();