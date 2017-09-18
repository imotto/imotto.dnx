namespace iMotto.Adapter.Users.Requests
{
    class AddRelAccountRequest:AuthedRequest
    {
        public int Platform { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
    }
}
