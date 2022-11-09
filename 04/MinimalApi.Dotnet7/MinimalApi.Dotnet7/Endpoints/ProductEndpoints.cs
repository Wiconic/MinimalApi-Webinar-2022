using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.OpenApi;

namespace MinimalApi.Dotnet7.Endpoints
{
    public static class ProductEndpoints
    {
        public static void RegisterProductEndpoints(this WebApplication app)
        {
            app.MapGet("/Products", GetAllProducts)
                .WithOpenApi();
            app.MapGet("/Products/{id}", GetProductById)
                .WithOpenApi();
            app.MapPost("/Products", AddProduct)
                .WithOpenApi();
            app.MapPut("/Products", UpdateProduct)
                .WithOpenApi();
            app.MapDelete("/Products/{id}", DeleteProduct)
                .WithOpenApi();
        }

        public static void ConfigureProductServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
        }

        private static List<Product> GetAllProducts(IProductService service)
        {
            return service.GetAllProducts().ToList();
        }

        private static IResult GetProductById(IProductService service ,int id)
        {
            var product = service.GetById(id);
            if(product == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        }

        private static IResult AddProduct(IProductService service, Product product)
        {
            var productAdded = service.AddProduct(product);
            return Results.Created($"/customers/{productAdded.Id}", product);                
        }

        private static IResult UpdateProduct(IProductService service, Product product)
        {
            if(service.UpdateProduct(product) != null)
            {
                return Results.Ok();
            }
            return Results.NotFound();
        }

        private static IResult DeleteProduct(int id, IProductService service)
        {
            if(service.RemoveProduct(id))
            {
                return Results.Ok();
            }
            return Results.NotFound();
        }
    }
}
