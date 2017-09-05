
namespace iMotto.Adapter
{
    /// <summary>
    /// 业务请求基类
    /// </summary>
    public class HandleRequest
    {
        /// <summary>
        /// 业务适配器代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///设备识别码 除设备注册和测速请求外，必须验证此请求码
        /// </summary>
        public string Sign { get; set; }
    }
}