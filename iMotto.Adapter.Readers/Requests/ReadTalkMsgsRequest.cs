namespace iMotto.Adapter.Readers.Requests
{
    class ReadTalkMsgsRequest:AuthedRequest
    {
        public string UID { get; set; }

        /// <summary>
        /// 起始消息ID，为0时，取最新的第一页。
        /// </summary>
        public long Start { get; set; }

        /// <summary>
        /// 当Take为负数时向前（旧）取，反之向后（新）取
        /// </summary>
        public int Take { get; set; }
    }
}
