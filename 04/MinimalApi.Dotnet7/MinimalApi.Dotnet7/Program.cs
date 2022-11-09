using Microsoft.AspNetCore.OpenApi;
using MinimalApi.Dotnet7.Authentication;
using MinimalApi.Dotnet7.Endpoints;
using MinimalApi.Dotnet7.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAuth();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureProductServicesWithFilters();


var app = builder.Build();

//app.RegisterProductEndpointsWithFilters();

//var productGroup = app.MapGroup("Product");
//productGroup.RegisterProductEndpointsGrouped();

//app.RegisterProductEndpointsGrouped();
app.RegisterAuthEndpoints();

app.RegisterSwaggerEndpoints();

app.UseHttpsRedirection();

app.Run();