namespace iMotto.Adapter
{
    public class HandleStates
    {
        public const int Success = 0;
        /// <summary>
        /// 未知错误
        /// </summary>
        public const int UnkownError = 101;
        /// <summary>
        /// 非法请求来源
        /// </summary>
        public const int InvalidSource = 102;
        /// <summary>
        /// 协议号不正确
        /// </summary>
        public const int InvalidAdapter = 103;
        /// <summary>
        /// 用户名已存在
        /// </summary>
        public const int UserAlreadyExists = 104;
        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        public const int InvalidUserNameOrPassword = 105;
        /// <summary>
        /// 验证码错误 
        /// </summary>
        public const int InvalidVerifyCode = 106;
        /// <summary>
        /// 非法安全认证
        /// </summary>
        public const int InvalidAuthorization = 107;
        /// <summary>
        /// 当前账户已在其它终端登录
        /// </summary>
        public const int DuplicatedLogin = 108;
        /// <summary>
        /// 手机号码已使用
        /// </summary>
        public const int DuplicatedPhone = 109;

        /// <summary>
        /// 输入的密码不合规则
        /// </summary>
        public const int InvalidPassword = 110;
        /// <summary>
        /// 用户名(手机号码)未注册
        /// </summary>
        public const int UserNotExists = 111;
        /// <summary>
        /// 不具有指定角色权限
        /// </summary>
        public const int InvalidRole = 112;

        /// <summary>
        /// 用户未登录，或签名不正确.
        /// </summary>
        public const int NotLoginYet = 113;

        /// <summary>
        /// 手机号码不合法.
        /// </summary>
        public const int InvalidPhone = 114;
        
        /// <summary>
        /// 输入的数据有误
        /// </summary>
        public const int InvalidData = 115;

        /// <summary>
        /// 没找到请求的信息
        /// </summary>
        public const int NoDataFound = 116;

        /// <summary>
        /// 请求的操作超时
        /// </summary>
        public const int TimeOut = 199;
    }
}
