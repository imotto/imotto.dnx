namespace iMotto.Adapter.Users.Requests
{
    class ExchangeRequest:AuthedRequest
    {
        public int GiftId { get; set; }
        public int Amount { get; set; }
        public long ReqInfoId { get; set; }

    }
}
