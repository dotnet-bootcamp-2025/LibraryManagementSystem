
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register LibraryService as a singleton
// Pregunta Entrevista: cuales son los 3 life-cycles con los que puedes inyectar instancias (Singleton, Scoped(se crea instancia a traves de request de contexto), Transient(se crea una instancia y se tira, es la más volatil de los 3))
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