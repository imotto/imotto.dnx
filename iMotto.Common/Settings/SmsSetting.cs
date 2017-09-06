namespace iMotto.Common.Settings
{
    class SmsSetting : ISmsSetting
    {
        public string AppKey { get; set; }

        public string ApiUrl { get; set; }

        public string AppSecret { get; set; }

        public string RegisterTemplateId { get; set; }

        public string ResetPassTemplateId { get; set; }
    }
}
