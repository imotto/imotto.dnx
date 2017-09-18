namespace iMotto.Adapter.Users.Requests
{
    class ModifyUserNameRequest:AuthedRequest
    {
        public string UserName { get; set; }
    }
}
