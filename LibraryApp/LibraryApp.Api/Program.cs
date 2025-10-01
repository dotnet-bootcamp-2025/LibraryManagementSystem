using LibraryApp.Infrastructure.Data;
using LibraryApp.Services;
using System;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
IServiceCollection serviceCollection = builder.Services.AddDbContext<AppDBContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register LibraryService as a singleton
// 3 lifecycles: Singleton, Scoped, Transient
//builder.Services.AddSingleton ILibraryService, LibraryService();

builder.Services.AddSingleton<LibraryApp.Services.ILibraryService, LibraryApp.Services.LibraryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // enable both OpenAPI and Swagger
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
