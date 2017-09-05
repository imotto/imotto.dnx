namespace iMotto.Data.Entities
{
    public enum ExchangeResult
    {
        /// <summary>
        /// 兑换成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 余额不足
        /// </summary>
        InsufficientBalance = 1,
        /// <summary>
        /// 兑换限制
        /// </summary>
        ReachLimit = 2,
        /// <summary>
        /// 库存不足
        /// </summary>
        SoldOut = 3,
        /// <summary>
        /// 并发限制
        /// </summary>
        ConcurrencyError = 4,
        /// <summary>
        /// 缺少信息
        /// </summary>
        RequireInfo = 5,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 6
    }
}
