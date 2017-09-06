namespace iMotto.Common.Settings
{
    public interface ISmsSetting
    {
        string AppKey { get; }
        string ApiUrl { get; }
        string AppSecret { get; }
        string RegisterTemplateId { get; }
        string ResetPassTemplateId { get; }

    }
}
