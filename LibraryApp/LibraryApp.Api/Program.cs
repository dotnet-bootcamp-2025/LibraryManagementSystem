
using LibraryApp.Infrastrucutre.Data;
using LibraryApp.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Register LibraryService as a singleton
// Pregunta Entrevista: cuales son los 3 life-cycles con los que puedes inyectar instancias (Singleton, Scoped(se crea instancia a traves de request de contexto), Transient(se crea una instancia y se tira, es la más volatil de los 3))
//builder.Services.AddScoped<ILibraryAppRepository, LibraryAppRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();