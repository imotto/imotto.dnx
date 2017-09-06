using iMotto.Common.Settings;
using MySql.Data.MySqlClient;
using System.Data;

namespace iMotto.Data.Dapper
{
    public class DapperConnectionProvider : IConnectionProvider
    {
        private readonly IDbSetting _dbSetting;

        public DapperConnectionProvider(ISettingProvider settingProvider)
        {
            _dbSetting = settingProvider.GetDbSetting();
        }

        public IDbConnection GetConnection()
        {
            var connection = new MySqlConnection(_dbSetting.ConnStr);
            connection.Open();

            return connection;
        }
    }
}
