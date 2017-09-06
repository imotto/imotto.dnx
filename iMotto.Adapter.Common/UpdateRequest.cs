namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 版本更新请求
    /// </summary>
    class UpdateRequest:HandleRequest
    {
        /// <summary>
        /// 类别 A.Android B.iPhone C.iPad;
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int Version { get; set; }

    }
}
