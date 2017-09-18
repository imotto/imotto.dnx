namespace iMotto.Adapter.Users.Requests
{
    class AddAddressRequest:AuthedRequest
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Contact { get; set; }
        public string Mobile { get; set; }
    }
}
