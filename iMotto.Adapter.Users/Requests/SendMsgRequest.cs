namespace iMotto.Adapter.Users.Requests
{
    class SendMsgRequest: AuthedRequest
    {
        public string TUID { get; set; }

        public string Content { get; set; }
    }
}
