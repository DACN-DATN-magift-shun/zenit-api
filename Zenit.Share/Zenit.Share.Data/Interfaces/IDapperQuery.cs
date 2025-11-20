using static Dapper.SqlMapper;

namespace Zenit.Share.Data.Interfaces
{
    public interface IDapperQuery
    {
        T? QuerySingle<T>(string sql, object? param = null) where T : class;
        IEnumerable<T> Query<T>(string sql, object? param = null) where T : class;
        GridReader QueryMultipleResults(string sql, object? param = null);
    }
}