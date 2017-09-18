namespace iMotto.Adapter.Users.Requests
{
    class PrepareExchangeReqeust:AuthedRequest
    {
        public int GiftId { get; set; }
        public int ReqInfoType { get; set; }
    }
}
