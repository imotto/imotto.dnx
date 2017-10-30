namespace iMotto.Common.Settings
{
    class OssSetting : IOssSetting
    {
        public string Endpoint { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string Region { get; set; }

        public string Bucket { get; set; }

        public string RoleArn { get; set; }

        public string HostName { get; set; }
    }
}
