using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register LibraryService as a singleton
// 3 life-cycles: Singleton, Scoped, Transient
builder.Services.AddScoped<ILibraryAppRepository, LibraryAppRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//Enable Swagger UI in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Redirect root URL to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
