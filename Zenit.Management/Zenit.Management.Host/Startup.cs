    using Mapster;

    using OpenTelemetry.Metrics;
    using OpenTelemetry.Resources;

    using Zenit.Management.Common.Models;
    using Zenit.Management.Data;
    using Zenit.Share.Common.Constants;
    using Zenit.Share.Data.Interfaces;
    using Zenit.Share.Host.Extensions;

    namespace Zenit.Management.Host
    {
        public class Startup(IConfiguration Configuration)
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddHttpContextAccessor();
                services.AddSwagger();

                services.AddCors();
                services.AddHttpClient();

                services.AddDbContext<ManagementDbContext>();

                services.AddMediatR((configs) =>
                {
                    configs.RegisterServicesFromAssemblyContaining<Startup>();
                });

                services.AddApplicationService();
                services.AddDomainService();

                services.AddAuthenticationService();
                services.AddAuthorization();

                services.AddCurrentAccount();
                services.AddScoped(typeof(IRepository<>), typeof(ManagementRepository<>));
                services.AddScoped<IUnitOfWork, ManagementUnitOfWork>();
                services.AddMapster();

                services.AddRabbitmqService();
                services.AddRabbitmqProducerService();

                services.AddDapperQuery();

                services.AddStackExchangeRedisCache(options =>
                {
                    options.InstanceName = Environment.GetEnvironmentVariable(EnvConstants.REDIS_CACHE_INSTANCE_NAME);
                    options.Configuration = Environment.GetEnvironmentVariable(EnvConstants.REDIS_CACHE_CONNECTION);
                });
                services.AddCacheService();
                // services.AddSendGrid();
                services.AddResend();
                services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(
                        serviceName: "zenit-management-api",
                        serviceVersion: "1.0.0"
                    )
                )
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()   // HTTP request metrics
                    .AddHttpClientInstrumentation()   // Outbound HTTP metrics
                    .AddRuntimeInstrumentation()      // GC, ThreadPool, Memory
                    .AddMeter("Npgsql")    // PostgreSQL metrics
                    .AddPrometheusExporter()          // Expose /metrics endpoint
                );
            }

            public void Configure(IApplicationBuilder app)
            {
                var isProduction = Environment.GetEnvironmentVariable("IS_PRODUCTION");
                if (isProduction == "false")
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                app.UseGlobalExceptionHandler();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();


                app.UseCurrentAccount<ManagementCurrentAccount>();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapPrometheusScrapingEndpoint("/metrics");
                });
            }
        }
    }
