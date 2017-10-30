namespace iMotto.Common.Settings
{
    class SettingKeys
    {
        /// <summary>
        /// 数据库连接的配置Key
        /// </summary>
        public const string DbConnStrKey = "ConnStr";

        /// <summary>
        /// Redis连接的配置Key
        /// </summary>
        public const string RedisConnStrKey = "RedisConnStr";

        /// <summary>
        /// token 相关配置
        /// </summary>
        public const string TokenIssuer = "TokenIssuer";

        public const string TokenAudience = "TokenAudience";
        public const string TokenExpires = "TokenExpiresInMinutes";
        public const string TokenSecretKey = "TokenSecretKey";

        /// <summary>
        /// 短信相关
        /// </summary>
        public const string SmsSettingKey = "SmsSetting";
        //public const string SmsAppKey = "SmsAppKey";
        //public const string SmsApiUrlKey = "SmsApiUrl";
        //public const string SmsAppSecretKey = "SmsAppSecret";
        //public const string SmsRegisterTemplateKey = "SmsRegTemplateId";
        //public const string SmsResetPassTemplateKey = "SmsResetTemplateId";
        


        /// <summary>
        /// 阿里云 oss
        /// </summary>
        public const string AliOssAccessKeyId = "AliAccessKeyId";

        public const string AliOssAccessKeySecret = "AliAccessKeySecret";
        public const string AliOssHost = "AliOssHost";
        public const string AliOssBucketName = "AliOssBucket";
        public const string AliOssDirectory = "AliOssDirectory";
        public const string AliOssRegion = "AliOssRegion";
        public const string AliOssRoleArn = "AliOssRoleArn";


        public const string OssSettingKey="OssSetting";
    }
}
