var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //se instaló la libreria swagger

// Register LibraryService as a singleton
// 3 lyfecycles: Singleton:en todo el ciclo de vida de la aplicacion, Scoped:por sesion, Transient:en el ciclo del request y recibio una respuesta.-Pregunta de entrevista la diferencia entre los 3
builder.Services.AddSingleton<LibraryApp.Services.LibraryService>();

var app = builder.Build();

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
