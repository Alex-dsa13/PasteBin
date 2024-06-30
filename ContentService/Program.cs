using ContentService.Base;
using ContentService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceLogging();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.RegisterServices();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandler>();
app.UseMiddleware<AuthHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
