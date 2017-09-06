using iMotto.Data.Entities;

namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 设备注册请求响应数据
    /// </summary>
    class RegisterDeviceResult:HandleResult
    {
        /// <summary>
        /// 设备识别码
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        ///  升级标识  不需升级：0，需要升级：1，强制升级：2	
        /// </summary>
        public int UpdateFlag { get; set; }

        /// <summary>
        /// NewVersion	新版本号	
        /// </summary>
        public string NewVersion { get; set; }

        /// <summary>
        /// UpdateSummary	更新说明	
        /// </summary>
        public string UpdateSummary { get; set; }

        /// <summary>
        /// DownloadUrl	下载地址
        /// </summary>
        public string DownloadUrl { get; set; }

        
        /// <summary>
        /// 附加信息
        /// </summary>
        public SpotLight Extra { get; set; }
    }
}
