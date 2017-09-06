namespace iMotto.Common.Settings
{
    public interface IDbSetting
    {
        string DbType { get; }
        string ConnStr { get; }
    }
}
