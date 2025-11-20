using Zenit.Share.Common.Constants;
using Zenit.Share.Data;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Statistics.Business
{
    public class DapperQueryService : DapperQueryBase
    {
        public DapperQueryService()
        {
            ConnectionString = GetConnectionString();
        }

        public string GetConnectionString()
        {
            var connectionName = EnvConstants.STATISTICS_CONNECTION ?? throw new Exception("Connection Name is not set.");
            var connectionString = Environment.GetEnvironmentVariable(connectionName) ?? throw new Exception("Connection String is not set.");
            return connectionString;
        }
    }
}