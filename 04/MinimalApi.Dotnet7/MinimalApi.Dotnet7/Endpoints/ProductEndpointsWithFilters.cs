using Core.Models;
using Core.Services;
using Core.Validators;
using FluentValidation;
using Microsoft.AspNetCore.OpenApi;
using MinimalApi.Dotnet7.Filters;

namespace MinimalApi.Dotnet7.Endpoints
{
    public static class ProductEndpointsWithFilters
    {
        public static void RegisterProductEndpointsWithFilters(this WebApplication app)
        {
            app.MapGet("/Products", GetAllProducts)
                .WithOpenApi()
                .AddEndpointFilter<ProductFilter>();

            app.MapGet("/Products/{id}", GetProductById)
                .WithOpenApi()
                .AddEndpointFilter<ProductFilter>();

            app.MapPost("/Products", AddProduct)
                .WithOpenApi()
                .AddEndpointFilter<ProductValidatorFilter>();

            app.MapPut("/Products", UpdateProduct)
                .WithOpenApi()
                .AddEndpointFilter<ProductValidatorFilter>()
                .AddEndpointFilter<ProductFilter>();
                
            app.MapDelete("/Products/{id}", DeleteProduct)
                .WithOpenApi();
        }

        public static void ConfigureProductServicesWithFilters(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IValidator<Product>, ProductValidator>();
        }

        private static List<Product> GetAllProducts(IProductService service)
        {
            return service.GetAllProducts().ToList();
        }

        private static Product? GetProductById(IProductService service, int id)
        {
            return service.GetById(id);
        }
        private static IResult AddProduct(IProductService service, Product product)
        {
            var productAdded = service.AddProduct(product);
            return Results.Created($"/customers/{productAdded.Id}", product);
        }

        private static Product? UpdateProduct(IProductService service, Product product)
        {
           return service.UpdateProduct(product);
        }

        private static IResult DeleteProduct(int id, IProductService service)
        {
            if(!service.RemoveProduct(id))
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }
    }
}
