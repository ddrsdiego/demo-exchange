namespace Demo.Exchange.Extensions.IoC
{
    using Demo.Exchange.Infra.Cache.Memcached;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Globalization;

    public static class DemoExchangeContainer
    {
        private const string CULTURE_INFO = "pt-BR";

        public static IServiceCollection AddDemoExchangeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJaeger();
            services.AddSwagger();
            services.AddServices();
            services.AddHandlers();
            services.AddOptions(configuration);
            services.AddMemcached(configuration);
            services.AddRepositories(configuration);

            return services;
        }

        public static IApplicationBuilder UseConfigDemoExchangeApp(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { new CultureInfo(CULTURE_INFO) };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: CULTURE_INFO, uiCulture: CULTURE_INFO),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }
    }
}