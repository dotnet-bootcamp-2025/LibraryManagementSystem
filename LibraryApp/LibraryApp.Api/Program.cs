

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register LibraryService as a singleton
// 3 lifecycles: Singleton, Scoped, Transient
//builder.Services.AddSingleton ILibraryService, LibraryService();

builder.Services.AddSingleton<LibraryApp.Console.Services.ILibraryService, LibraryApp.Console.Services.LibraryService>();

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
