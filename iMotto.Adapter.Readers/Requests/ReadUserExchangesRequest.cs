namespace iMotto.Adapter.Readers.Requests
{
    class ReadUserExchangesRequest:AuthedRequest
    {
        public int PIndex { get; set; }

        public int PSize { get; set; }
    }
}
