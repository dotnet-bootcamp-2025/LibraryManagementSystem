using LibraryApp.Console.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register library service as a singleton
// 3 lyfecycle singleton (toda la vida de la app en runtime)
// scoped (solo a lo largo de un contexto, porejemplo un request que va a ejecutar este controlador),
// transient (solo por la transaccion)
builder.Services.AddSingleton<ILibraryService, LibraryService>();


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
