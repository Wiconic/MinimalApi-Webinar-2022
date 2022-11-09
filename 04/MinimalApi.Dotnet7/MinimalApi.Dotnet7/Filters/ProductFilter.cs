namespace MinimalApi.Dotnet7.Filters
{
    public class ProductFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context);

            if (result == null)
            {
                return Results.NotFound();
            }
            
            return Results.Ok(result);
        }
    }
}
