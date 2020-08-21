namespace Demo.Consumer
{
    using Jaeger;
    using Jaeger.Samplers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using OpenTracing;
    using OpenTracing.Contrib.NetCore.CoreFx;
    using OpenTracing.Util;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .WithLoggerFactory(loggerFactory)
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                if (GlobalTracer.IsRegistered())
                    GlobalTracer.Register(tracer);

                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options => options.IgnorePatterns.Add(x => !x.RequestUri.IsLoopback));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}