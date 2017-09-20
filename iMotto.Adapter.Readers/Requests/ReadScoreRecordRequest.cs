namespace iMotto.Adapter.Readers.Requests
{
    class ReadScoreRecordRequest : AuthedRequest
    {
        public int PIndex { get; set; }

        public int PSize { get; set; }
    }
}
