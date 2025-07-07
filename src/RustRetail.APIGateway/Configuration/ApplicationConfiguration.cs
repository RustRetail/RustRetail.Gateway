namespace RustRetail.APIGateway.Configuration
{
    internal static class ApplicationConfiguration
    {
        internal static WebApplication ConfigureApplicationPipeline(
            this WebApplication app)
        {
            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            app.MapReverseProxy();

            return app;
        }
    }
}
