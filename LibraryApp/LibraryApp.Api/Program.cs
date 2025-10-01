using LibraryApp.Infraestructure.Data;
using LibraryApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //se instaló la libreria swagger

//configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Register LibraryService as singleton to maintain state across requests
// 3 lyfecycles: Singleton:en todo el ciclo de vida de la aplicacion, Scoped:por sesion, Transient:en el ciclo del request y recibio una respuesta.-Pregunta de entrevista la diferencia entre los 3
builder.Services.AddSingleton<ILibraryService, LibraryService>();

var app = builder.Build();
//seed data once at startup
using (var scope = app.Services.CreateScope())
{
    var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
    libraryService.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Enable both OpenAPI and Swagger UI
    app.MapOpenApi();
    app.UseSwagger();//ahora se usa swagger
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
