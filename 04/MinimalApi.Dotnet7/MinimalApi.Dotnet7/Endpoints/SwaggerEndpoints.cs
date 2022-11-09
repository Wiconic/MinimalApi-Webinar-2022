using Microsoft.OpenApi.Models;

namespace MinimalApi.Dotnet7.Endpoints
{
    public static class SwaggerEndpoints
    {
        public static void RegisterSwaggerEndpoints(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security",
            };

            var securityReq = new OpenApiSecurityRequirement()
                {
                    {
                         new OpenApiSecurityScheme{
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                         }
                     },
                     new string[] {}
                    }
                };

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
                {
                    o.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "SampleAuth0",
                        Version = "v1"
                    });
                    o.AddSecurityDefinition("Bearer", securityScheme);
                    o.AddSecurityRequirement(securityReq);
                });
        }
    }
}
