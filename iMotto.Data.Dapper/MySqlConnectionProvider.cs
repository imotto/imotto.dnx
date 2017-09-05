using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace iMotto.Data.Dapper
{
    public class MySqlConnectionProvider : IConnectionProvider
    {
        private string _connectionString;

        public MySqlConnectionProvider()
        {
            _connectionString = "";
        }

        public IDbConnection GetConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}
