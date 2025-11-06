using Hangfire;
using Hangfire.PostgreSql;

using Mapster;

using Zenit.Scheduler.Data;
using Zenit.Share.Common.Constants;
using Zenit.Share.Data.Interfaces;
using Zenit.Share.Host.Extensions;

namespace Zenit.Scheduler.Host
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchedulerDbContext>();

            services.AddCors();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            services.AddApplicationService();
            services.AddDomainService();
            services.AddScoped(typeof(IRepository<>), typeof(SchedulerRepository<>));
            services.AddScoped<IUnitOfWork, SchedulerUnitOfWork>();
            services.AddMapster();
            services.AddHangfire(cfg => cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(
                        Environment.GetEnvironmentVariable(EnvConstants.SCHEDULER_CONNECTION)
                    );
                })
            );
            services.AddHangfireServer(options => options.WorkerCount = 5);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("");
        }
    }
}