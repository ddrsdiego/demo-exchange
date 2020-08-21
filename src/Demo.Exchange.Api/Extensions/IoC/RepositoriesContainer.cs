namespace Demo.Exchange.Extensions.IoC
{
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Cache;
    using Demo.Exchange.Infra.Cache.Redis;
    using Demo.Exchange.Infra.Connectors;
    using Demo.Exchange.Infra.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

    internal static class RepositoriesContainer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConnectors();
            services.AddScoped<ITaxaCobrancaRepository, TaxaCobrancaRepository>();

            services.AddSingleton<IConnectionMultiplexer>(c =>
                ConnectionMultiplexer.Connect(configuration.GetValue<string>("ConnectionStrings:RedisConnectionString")));
            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");

            return services;
        }

        private static IServiceCollection AddConnectors(this IServiceCollection services)
        {
            services.AddTransient<InjectOpenTracingHeaderHandler>();

            services.AddScoped<IValuesApiConnector, ValuesApiConnector>();
            services.AddScoped<IExchangeRatesApiConnector, ExchangeRatesApiConnector>();

            services.AddHttpClient(nameof(ValuesApiConnector)).AddHttpMessageHandler<InjectOpenTracingHeaderHandler>();
            services.AddHttpClient(nameof(ExchangeRatesApiConnector)).AddHttpMessageHandler<InjectOpenTracingHeaderHandler>();

            return services;
        }
    }
}