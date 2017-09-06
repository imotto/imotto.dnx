using System;

namespace iMotto.Common.Settings
{
    class DbSetting : IDbSetting
    {
        public string DbType => "mysql";

        public string ConnStr
        {
            get; set;
        }
    }
}

