using iMotto.Common.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iMotto.Service
{
    public class DayuSmsService:ISmsService
    {
        private const string SIGN_METHOD_MD5 = "md5";
        private const string SIGN_METHOD_HMAC = "hmac";
        private readonly ISmsSetting _smsSetting;
        private readonly ILogger _logger;

        public DayuSmsService(ISettingProvider settingProvider, ILoggerFactory loggerFactory)
        {
            _smsSetting = settingProvider.GetSmsSetting();
            _logger = loggerFactory.CreateLogger<DayuSmsService>();
        }


        public async Task<bool> SendMsg(string dest, string template, Dictionary<string, string> para)
        {
            try
            {
                string url = MakeRequestUrl(dest, template, para);

                var client = new HttpClient();

                var response = await client.GetAsync(url);
                response = response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("request sms url:{0}, response:{1}", url, content);

                var jbj = JsonConvert.DeserializeObject<JObject>(content);

                var token = jbj.SelectToken("alibaba_aliqin_fc_sms_num_send_response.result.success");
                var success = token.Value<bool>();

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured during send sms to [{0}], Ex:{1}", dest, ex);
                return false;
            }
        }

        private string MakeRequestUrl(string dest, string template, Dictionary<string, string> para)
        {
            var paras = new Dictionary<string, string>();
            paras.Add("method", "alibaba.aliqin.fc.sms.num.send");
            paras.Add("app_key", _smsSetting.AppKey);
            paras.Add("sign_method", SIGN_METHOD_MD5);
            paras.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            paras.Add("v", "2.0");
            paras.Add("format", "json");

            paras.Add("sms_type", "normal");
            paras.Add("sms_free_sign_name", "偶得");
            paras.Add("sms_param", JsonConvert.SerializeObject(para));
            paras.Add("rec_num", dest);
            paras.Add("sms_template_code", template);

            var sign = SignTopRequest(paras, _smsSetting.AppSecret, SIGN_METHOD_MD5);

            Console.WriteLine(sign);

            paras.Add("sign", sign);

            StringBuilder queryStrings = new StringBuilder();

            foreach (var kvp in paras)
            {
                queryStrings.Append("&");
                queryStrings.Append(kvp.Key);
                queryStrings.Append("=");
                queryStrings.Append(SpecialUrlEncode(kvp.Value));
            }
            
            var url = _smsSetting.ApiUrl + "?" +
                              queryStrings.ToString(1, queryStrings.Length - 1);
            return url;
        }

        private static string GetBaseUrl(string url)
        {
            var uri = new Uri(url);

            if (uri.IsDefaultPort)
            {
                return $"{uri.Scheme}://{uri.Host}";
            }

            return $"{uri.Scheme}://{uri.Host}:{uri.Port}";
        }

        private static string SpecialUrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8).Replace("+", "%20").Replace("*", "%2A")
                 .Replace("%7E", "~");
        }

        private static string SignTopRequest(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            dem.Dispose();

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (SIGN_METHOD_HMAC.Equals(signMethod))
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
