namespace RustRetail.APIGateway.Configuration
{
    internal static class ServicesConfiguration
    {
        const string ReverseProxySectionName = "ReverseProxy";
        const string JwtSectionName = "Jwt";
        const string JwtAuthoritySectionName = "Authority";
        const string JwtAudienceSectionName = "Audience";
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
                    // Identity Service base URL
                    options.Authority = configuration.GetSection(JwtSectionName)[JwtAuthoritySectionName];
                    // Only for development
                    options.RequireHttpsMetadata = true;
                    options.Audience = configuration.GetSection(JwtSectionName)[JwtAudienceSectionName];

                    // Allow self-signed certificates for development purposes
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });

            return services;
        }
    }
}
