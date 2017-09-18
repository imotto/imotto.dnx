namespace iMotto.Adapter.Users.Requests
{
    class ReceiveAwardRequest:AuthedRequest
    {
        public int AwardId { get; set; }
    }
}
