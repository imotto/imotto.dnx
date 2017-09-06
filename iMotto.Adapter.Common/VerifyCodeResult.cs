namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 获取验证码响应数据
    /// </summary>
    class VerifyCodeResult:HandleResult
    {
        /// <summary>
        /// 验证码内容
        /// </summary>
        public string Content { get; set; }
    }
}
