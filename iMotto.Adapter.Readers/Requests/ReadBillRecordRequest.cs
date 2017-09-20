namespace iMotto.Adapter.Readers.Requests
{
    class ReadBillRecordRequest : AuthedRequest
    {

        public int PIndex { get; set; }

        public int PSize { get; set; }
    }
}
