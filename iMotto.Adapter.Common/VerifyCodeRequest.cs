namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 获取验证码请求
    /// </summary>
    class VerifyCodeRequest:HandleRequest
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        
        /// <summary>
        /// 操作类型， 1：用户注册，2：找回密码
        /// </summary>
        public int OpCode { get; set; }
    }
}
