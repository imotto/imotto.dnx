namespace iMotto.Adapter
{
    /// <summary>
    /// 需要认证用户的业务请求基类
    /// </summary>
    public class AuthedRequest : HandleRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}