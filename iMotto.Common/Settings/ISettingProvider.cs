using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Common.Settings
{
    public interface ISettingProvider
    {
        Task<string> GetSetting(string key, bool throwWhenError = true);

        Task<T> GetSetting<T>(string key, bool throwWhenError = true) where T : new();

        ISmsSetting GetSmsSetting();

        IDbSetting GetDbSetting();

        ICacheSetting GetCacheSetting();

        IOssSetting GetOssSetting();
    }
}
