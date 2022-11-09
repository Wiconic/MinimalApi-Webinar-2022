using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApi.Dotnet7.Authentication
{


    public static class AuthEndpoints
    {
        public static void RegisterAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/login", Login);

            app.MapGet("SecureGetMessage", () => "This is a secure link!").RequireAuthorization();
            app.MapGet("AdminMessage", () => "This is a secure admin link!").RequireAuthorization("admin");

            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void ConfigureAuth(this WebApplicationBuilder builder)
        {
            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

            builder.Services.ConfigureAuthServices(jwtOptions);
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        }

        private static void ConfigureAuthServices(this IServiceCollection services, JwtOptions options)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("admin", p => p.
                       RequireAuthenticatedUser().
                       RequireClaim("admin", "True"));
            });
        }

        private static IResult Login(User user, IOptions<JwtOptions> options)
        {
            if ((user.UserName.Equals("Jeroen", StringComparison.OrdinalIgnoreCase) && user.Password == "Jeroen1234")
                || (user.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase) && user.Password == "SuperSecret"))
            {
                var issuer = options.Value.Issuer;
                var audience = options.Value.Audience;
                var key = Encoding.ASCII.GetBytes(options.Value.Key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new[]
                    {
                                    new Claim("admin", user.UserName.Equals("Admin").ToString()),
                                    new Claim("Id", Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                                    new Claim(JwtRegisteredClaimNames.Jti,
                                    Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                return Results.Ok(stringToken);
            }
            return Results.Unauthorized();
        }
    }
}
