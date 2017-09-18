namespace iMotto.Adapter.Users.Requests
{
    class ReviewGiftRequest:AuthedRequest
    {
        public int GiftId { get; set; }
        public long ExchangeId { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; }
    }
}
