using Mapster;

using StackExchange.Redis;

using Zenit.Share.Common.Constants;
using Zenit.Share.Data.Interfaces;
using Zenit.Share.Host.Extensions;
using Zenit.Statistics.Business;
using Zenit.Statistics.Business.Workers;
using Zenit.Statistics.Common.Models;
using Zenit.Statistics.Data;

namespace Zenit.Statistics.Host
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddSwagger();

            services.AddCors();
            services.AddHttpClient();

            services.AddDbContext<StatisticsDbContext>();

            services.AddMediatR((configs) =>
            {
                configs.RegisterServicesFromAssemblyContaining<Startup>();
            });
            services.AddApplicationService();
            services.AddDomainService();

            services.AddAuthenticationService();
            services.AddCurrentAccount();
            services.AddScoped(typeof(IRepository<>), typeof(StatisticsRepository<>));
            services.AddScoped<IUnitOfWork, StatisticsUnitOfWork>();
            services.AddMapster();

            services.AddRabbitmqService();
            services.AddRabbitmqProducerService();
            services.AddRabbitmqConsumerService();
            services.AddDapperQuery();
            services.AddCacheService();

            services.AddHostedService<BackgroundConsumer>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = Environment.GetEnvironmentVariable(EnvConstants.REDIS_CACHE_INSTANCE_NAME);
                options.Configuration = Environment.GetEnvironmentVariable(EnvConstants.REDIS_CACHE_CONNECTION);
            });
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

            app.UseCurrentAccount<StatisticsCurrentAccount>();  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
