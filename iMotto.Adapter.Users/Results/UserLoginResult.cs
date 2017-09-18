namespace iMotto.Adapter.Users.Results
{
    class UserLoginResult : HandleResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserThumb { get; set; }
        public string UserToken { get; set; }
    }
}
