using Core.Models;
using FluentValidation;

namespace MinimalApi.Dotnet7.Filters
{
    public class ProductValidatorFilter : IEndpointFilter
    {
        private readonly IValidator<Product> validator;

        public ProductValidatorFilter(IValidator<Product> validator)
        {
            this.validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var param = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(Product));

            if( param is Product product)
            { 
                var result = await validator.ValidateAsync(product);

                if(!result.IsValid)
                {
                    return Results.BadRequest(result.Errors);
                }
            }

            return await next(context);
        }
    }
}
