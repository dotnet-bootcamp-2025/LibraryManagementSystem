//program.scs library.app.api
using LibraryApp.Api;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Services;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
{
    
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
     {
         // Aquí registras tu convertidor personalizado
         options.JsonSerializerOptions.Converters.Add(new DateConverter());
     });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// register library service as a singleton
// 3 life-cycles singleton, scoped, transient
// register as singleton to maintain state across requests
builder.Services.AddScoped<ILibraryAppRepository, LibraryAppRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable both OpenAPI and Swagger UI
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
