using Zenit.Accounts.Data;
using Zenit.Share.Common.Constants;


namespace Zenit.Accounts.Host.Extensions
{
    public static class AccountServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            services.AddSingleton<AccountDbContext>(provider =>
            {   
                var connectionString = Environment.GetEnvironmentVariable(EnvConstants.CONNECTION_STRING);
                var databaseName = Environment.GetEnvironmentVariable(EnvConstants.DATABASE);

                return new AccountDbContext(
                    connectionString ?? throw new Exception("MongoDB connection string is not set."),
                    databaseName ?? throw new Exception("MongoDB database name is not set."));
            });
            
            return services;
        }
    }
}
