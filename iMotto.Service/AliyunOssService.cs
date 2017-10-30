using Aliyun.OSS;
using iMotto.Common.Settings;
using Microsoft.AspNetCore.Hosting;
using System;

namespace iMotto.Service
{
    public class AliyunOssService : IObjectStorageService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IOssSetting _ossSetting;
        private readonly bool _isProductionEnv;

        public AliyunOssService(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
            _ossSetting = settingProvider.GetOssSetting();
        }

        public string UploadFile(string key, string filePath)
        {
            OssClient client = new OssClient(
                _ossSetting.Endpoint,
                _ossSetting.AppId,
                _ossSetting.AppSecret);
            
            var result = client.PutObject(_ossSetting.Bucket, key, filePath);

            return $"{_ossSetting.HostName}/{key}";
        }
    }
}
