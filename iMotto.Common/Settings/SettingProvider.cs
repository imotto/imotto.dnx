using Consul;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Common.Settings
{
    public class SettingProvider : ISettingProvider
    {
        private readonly ConsulSetting _options;
        private readonly ConsulClient _consul;

        public SettingProvider(IOptions<ConsulSetting> options)
        {
            _options = options.Value;

            var consulHost = string.IsNullOrEmpty(_options.ConsulHost) ? "localhost" : _options.ConsulHost;
            var consulPort = options.Value.ConsulPort ?? 8500;

            _consul = new ConsulClient(config =>
            {
                config.Address = new Uri($"http://{consulHost}:{consulPort}");
                if (!string.IsNullOrWhiteSpace(options.Value.Token))
                {
                    config.Token = options.Value.Token;
                }
            });
        }


        public async Task<string> GetSetting(string key, bool throwWhenError = true)
        {
            var x = await _consul.KV.Get($"{_options.Folder}/{key}");
            if (x.StatusCode != HttpStatusCode.OK)
            {
                if (throwWhenError)
                {
                    throw new SettingException($"unknown setting key: {key}");
                }

                return null;
            }

            if (x.Response.Value.Length > 0)
            {
                return Encoding.UTF8.GetString(x.Response.Value);
            }

            return string.Empty;
        }

        public async Task<T> GetSetting<T>(string key, bool throwWhenError = true) where T : new()
        {
            var setting = await GetSetting(key, throwWhenError);

            if (string.IsNullOrWhiteSpace(setting))
                return default(T);

            return JsonConvert.DeserializeObject<T>(setting);
        }

        public ISmsSetting GetSmsSetting()
        {
            return GetSetting<SmsSetting>(SettingKeys.SmsSettingKey).GetAwaiter().GetResult();
        }


        public IDbSetting GetDbSetting()
        {
            var connstr = GetSetting(SettingKeys.DbConnStrKey).GetAwaiter().GetResult();

            return new DbSetting
            {
                ConnStr = connstr
            };
        }

        public ICacheSetting GetCacheSetting()
        {
            var redisConnStr = GetSetting(SettingKeys.RedisConnStrKey).GetAwaiter().GetResult();

            return new CacheSetting
            {
                RedisConnStr = redisConnStr
            };
        }
        

        //get service method not implemented
    }
}