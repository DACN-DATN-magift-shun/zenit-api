using Dapper;

using Npgsql;

using Zenit.Share.Common.Constants;
using Zenit.Share.Data.Interfaces;

using static Dapper.SqlMapper;

namespace Zenit.Share.Data
{
    public abstract class DapperQueryBase : IDapperQuery
    {
        public string ConnectionString { get; set; }
        protected NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public T? QuerySingle<T>(string sql, object? param = null) where T : class
        {
            using var connection = GetConnection();
            connection.Open();
            var result = connection.QuerySingleOrDefault<T>(sql, param);
            return result;
        }

        public IEnumerable<T> Query<T>(string sql, object? param = null) where T : class
        {
            using var connection = GetConnection();
            connection.Open();
            var result = connection.Query<T>(sql, param);
            return result;
        }

        public GridReader QueryMultipleResults(string sql, object? param = null)
        {
            using var connection = GetConnection();
            connection.Open();
            var result = connection.QueryMultiple(sql, param);
            return result;
        }

        public void Execute(string sql, object? param = null)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute(sql, param);
        }

        public T QueryScalar<T>(string sql, object? param = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.ExecuteScalar<T>(sql, param);
        }
    }
}