namespace iMotto.Adapter.Users.Requests
{
    class AddUserRequest : HandleRequest
    {
        /// <summary>
        /// 手机号码，必填 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 登录密码, 必填
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户名，
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string InviteCode { get; set; }
    }
}
