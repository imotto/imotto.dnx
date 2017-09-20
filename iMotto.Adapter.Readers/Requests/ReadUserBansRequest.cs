namespace iMotto.Adapter.Readers.Requests
{
    class ReadUserBansRequest:AuthedRequest
    {
        public int PIndex { get; set; }
        public int PSize { get; set; }
    }
}
