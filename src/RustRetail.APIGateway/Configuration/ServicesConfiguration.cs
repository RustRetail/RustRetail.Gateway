using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RustRetail.APIGateway.Configuration
{
    internal static class ServicesConfiguration
    {
        const string ReverseProxySectionName = "ReverseProxy";
        const string JwtSectionName = "Jwt";
        const string JwtIssuerSectionName = "Issuer";
        const string JwtAudienceSectionName = "Audience";
        const string JwtSecretKeySectionName = "SecretKey";
        const string JwtAuthenticationScheme = "Bearer"; 

        internal static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureReverseProxy(configuration);
            services.ConfigureJwtAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        static IServiceCollection ConfigureReverseProxy(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection(ReverseProxySectionName));

            return services;
        }

        static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtAuthenticationScheme)
                .AddJwtBearer(JwtAuthenticationScheme, options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetSection(JwtSectionName)[JwtIssuerSectionName],
                        ValidAudience = configuration.GetSection(JwtSectionName)[JwtAudienceSectionName],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(JwtSectionName)[JwtSecretKeySectionName]!)),
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = "roles"
                    };
                });

            return services;
        }
    }
}
