using Zenit.Share.Common.Constants;
using Zenit.Share.Data;

namespace Zenit.Management.Business
{
    public class ManagementDapperQueryService : DapperQueryBase
    {
        public ManagementDapperQueryService()
        {
            ConnectionString = GetConnectionString();
        }

        public string GetConnectionString()
        {
            var connectionName = EnvConstants.MANAGEMENT_CONNECTION ?? throw new Exception("Connection Name is not set.");
            var connectionString = Environment.GetEnvironmentVariable(connectionName) ?? throw new Exception("Connection String is not set.");
            return connectionString;
        }
    }
}