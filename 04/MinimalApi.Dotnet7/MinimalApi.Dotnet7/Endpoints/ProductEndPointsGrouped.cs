using Core.Models;
using Core.Services;
using Core.Validators;
using FluentValidation;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using MinimalApi.Dotnet7.Filters;
using System.ComponentModel;

namespace MinimalApi.Dotnet7.Endpoints
{
    public static class ProductEndPointsGrouped
    {
        public static RouteGroupBuilder RegisterProductEndpointsGrouped(this RouteGroupBuilder routeBuilder)
        {
            routeBuilder.MapGet("/", GetAllProducts);

            routeBuilder.MapGet("/{id}", GetProductById);

            var subgroup = routeBuilder.MapGroup("/");
            subgroup.MapPost("/", AddProduct);
            subgroup.MapPut("/", UpdateProduct);
            subgroup.AddEndpointFilter<ProductValidatorFilter>();

            routeBuilder.MapDelete("/{id}", DeleteProduct);

            routeBuilder.WithOpenApi();
            routeBuilder.AddEndpointFilter<ProductFilter>();

            return routeBuilder;
        }

        public static void RegisterProductEndpointsGrouped(this WebApplication app)
        {
            var productGroup = app.MapGroup("/Products");

            productGroup.RegisterProductEndpointsGrouped();
        }

        public static void ConfigureProductServicesGrouped(this IServiceCollection services)
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
            if (!service.RemoveProduct(id))
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }

    }
}
