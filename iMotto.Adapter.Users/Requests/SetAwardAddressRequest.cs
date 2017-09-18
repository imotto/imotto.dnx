namespace iMotto.Adapter.Users.Requests
{
    class SetAwardAddressRequest:AuthedRequest
    {
        public int AwardId { get; set; }
        public long AddrId { get; set; }
    }
}
