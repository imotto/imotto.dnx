using System.Data.SqlClient;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class RepoBase
    {
        //public RepoBase(string conn = "DefaultConnection")
        //{
        //    connStr = ConfigurationManager.ConnectionStrings[conn].ConnectionString;
        //}

        //public async Task<int> ExecuteNonQueryAsync(string connStr, string sql, params SqlParameter[] parameters)
        //{
        //    using (var conn = new SqlConnection(connStr))
        //    {
        //        await conn.OpenAsync();
        //        var cmd = conn.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.Parameters.AddRange(parameters);

        //        return await cmd.ExecuteNonQueryAsync();
        //    }
        //}

        //public async Task<SqlDataReader> ExecuteReaderAsync(string connStr, string sql, params SqlParameter[] paramters)
        //{
        //    using (var conn = new SqlConnection(connStr))
        //    {
        //        await conn.OpenAsync();
        //        var cmd = conn.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.Parameters.AddRange(paramters);

        //        return await cmd.ExecuteReaderAsync();
        //    }
        //}
    }
}