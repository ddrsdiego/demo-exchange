namespace Demo.Exchange.BenchmarkTest
{
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using Demo.Exchange.Extensions.IoC;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;

    internal static class ContainerConfiguration
    {
        public static IConfigurationRoot configuration;

        public static ServiceProvider Configure()
            => new ServiceCollection().ConfigureServices().BuildServiceProvider();

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddSingleton(configuration)
                    .AddDemoExchangeServices(configuration)
                    .AddMediatR(typeof(ObterTaxaCobrancaPorSegmentoQuery).Assembly);
            return services;
        }
    }
}