using AuthService.Base;
using AuthService.Controllers.Grpc;
using AuthService.Middlewares;
using AuthService.Properties;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceLogging();
builder.Services.AddControllers();
builder.Services.RegisterServices();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcReflection().AddGrpc();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandler>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcAuthController>();
app.MapGrpcReflectionService();

app.Run();
