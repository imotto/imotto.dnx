namespace iMotto.Adapter.Users.Requests
{
    class SetDefaultAddressRequest:AuthedRequest
    {
        public long AddrId { get; set; }
    }
}
