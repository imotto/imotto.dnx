namespace iMotto.Adapter.Mottos.Requests
{
    class UpdateCollectionRequest:AuthedRequest
    {
        public long CID { get; set; }
        public string Summary { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
    }
}
