using MinimalApi.Structure.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();

var app = builder.Build();

app.UseHttpsRedirection();

app.DefineEndpoints();

app.Run();
