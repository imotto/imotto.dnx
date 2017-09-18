namespace iMotto.Adapter.Users.Requests
{
    class AddReportRequest:HandleRequest
    {
        public string TargetID { get; set; }

        public int Type { get; set; }

        public string Reason { get; set; }
    }
}
