using Mapster;

using Zenit.Management.Common.Models;
using Zenit.Management.Data;
using Zenit.Share.Data.Interfaces;
using Zenit.Share.Host.Extensions;

namespace Zenit.Management.Host
{
    public class Startup(IConfiguration Configuration)
    {
        public async Task ConfigureServices(IServiceCollection services)
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

            services.AddCurrentAccount();
            services.AddScoped(typeof(IRepository<>), typeof(ManagementRepository<>));
            services.AddScoped<IUnitOfWork, ManagementUnitOfWork>();
            services.AddMapster();

            await services.AddRabbitmqService();
            services.AddRabbitmqProducerService();

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
            });
        }
    }
}
