namespace MinimalApi.Structure.Reflection
{
    public static class EndpointDefinitionResolver
    {
        public static void RegisterServices(
            this IServiceCollection services)
        {
            var endpointDefinitions = new List<IEndpointDefinition>();

            var type = typeof(IEndpointDefinition);

            endpointDefinitions.AddRange(
                type.Assembly.ExportedTypes
                    .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance).Cast<IEndpointDefinition>()
                    );

            foreach (var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineServices(services);
            }

            services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
        }

        public static void DefineEndpoints(this WebApplication app)
        {
            var endpointDefinitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

            foreach(var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.DefineEndpoints(app);
            }
        }
    }
}
