using Mapster;

using Zenit.Accounts.Common.Models;
using Zenit.Accounts.Data;
using Zenit.Share.Common.Constants;
using Zenit.Share.Data.Interfaces;
using Zenit.Share.Host.Extensions;


namespace Zenit.Accounts.Host
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddDbContext<AccountDbContext>();

            services.AddSwagger();

            services.AddCors();
            services.AddHttpClient();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            services.AddApplicationService();
            services.AddDomainService();

            services.AddAuthenticationService();

            services.AddCurrentAccount();
            services.AddScoped(typeof(IRepository<>), typeof(AccountRepository<>));
            services.AddScoped<IUnitOfWork, AccountUnitOfWork>();
            services.AddMapster();
        }

        public void Configure(IApplicationBuilder app)
        {
            var isProduction = Environment.GetEnvironmentVariable(EnvConstants.IS_PRODUCTION);
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

            app.UseCurrentAccount<CurrentAccount>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
