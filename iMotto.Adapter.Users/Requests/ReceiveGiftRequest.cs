namespace iMotto.Adapter.Users.Requests
{
    class ReceiveGiftRequest:AuthedRequest
    {
        public long ExchangeId { get; set; }
    }
}
