namespace iMotto.Common.Settings
{
    public interface IOssSetting
    {
        string Endpoint { get; }

        string AppId { get; }

        string AppSecret { get; }   
        
        string Region { get; }

        string Bucket { get; }    
        
        string RoleArn { get; }
        
        string HostName { get; }
    }
}
