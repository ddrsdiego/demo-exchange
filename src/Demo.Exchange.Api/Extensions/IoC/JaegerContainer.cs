namespace Demo.Exchange.Extensions.IoC
{
    using Demo.Exchange.Infra.Options;
    using Jaeger;
    using Jaeger.Samplers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OpenTracing;
    using OpenTracing.Contrib.NetCore.CoreFx;
    using OpenTracing.Util;
    using System;
    using System.Reflection;

    internal static class JaegerContainer
    {
        private static readonly Uri _jaegerUri = new Uri("http://localhost:14268/api/traces");

        public static IServiceCollection AddJaeger(this IServiceCollection services)
        {
            services.AddOpenTracing();
            services.AddSingleton(serviceProvider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var jaegerOptions = serviceProvider.GetRequiredService<IOptions<JaegerOptions>>();

                Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", serviceName);
                Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", jaegerOptions.Value.Host);
                Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", jaegerOptions.Value.Port);

                ITracer tracer = new Tracer.Builder(serviceName)
                                        .WithLoggerFactory(loggerFactory)
                                        .WithSampler(new ConstSampler(sample: true))
                                        .Build();

                if (!GlobalTracer.IsRegistered())
                    GlobalTracer.Register(tracer);

                return tracer;
            });

            //Evita loops infinitos quando o OpenTracing estiver rastreando solicitações HTTP para o Jaeger.
            services.Configure<HttpHandlerDiagnosticOptions>(options =>
            {
                options.IgnorePatterns.Add(x => !x.RequestUri.IsLoopback);
                options.IgnorePatterns.Add(request => _jaegerUri.IsBaseOf(request.RequestUri));
            });

            return services;
        }
    }
}