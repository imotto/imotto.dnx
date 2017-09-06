namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 设备注册业务请求
    /// </summary>
    class RegisterDeviceRequest:HandleRequest
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string OSVersion { get; set; }
        /// <summary>
        /// 屏幕尺寸
        /// </summary>
        public string Screen { get; set; }
        /// <summary>
        /// 分辩率
        /// </summary>
        public string Resolution { get; set; }
        /// <summary>
        /// 屏幕密度
        /// </summary>
        public string Midu { get; set; }
        /// <summary>
        /// 设备识别号
        /// </summary>
        public string UniqueId { get; set; }
        /// <summary>
        /// 系统ID号
        /// </summary>
        public string OSId { get; set; }
        /// <summary>
        /// 终端版本号
        /// </summary>
        public string TVersion { get; set; }
        /// <summary>
        /// 终端类型 A.Android B.iPhone C.iPad
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 当前应用版本
        /// </summary>
        public string Version { get; set; }
    }
}
