using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class DeviceReg
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 业务适配器
        /// </summary>
        public string Codes { get; set; }
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
        public string OprSystem { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemVer { get; set; }
        /// <summary>
        /// 屏幕尺寸
        /// </summary>
        public string ScreenSize { get; set; }
        /// <summary>
        /// 分辩率
        /// </summary>
        public string ResolutionRatio { get; set; } 
        /// <summary>
        /// 屏幕密度
        /// </summary>
        public string ScreenDesity { get; set; }
        /// <summary>
        /// 设备识别号 --Android对应IMEI，iPhone对应设备号			
        /// </summary>
        public string DevIdenNo { get; set; }
        /// <summary>
        /// 系统ID号
        /// </summary>
        public string SystemID { get; set; }
        /// <summary>
        /// 终端版本号
        /// </summary>
        public string TerminalVersion { get; set; }
        /// <summary>
        /// 当前应用版本
        /// </summary>
        public string CurrentVersion { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 设备签名
        /// </summary>
        public string DeviceSig { get; set; }
        /// <summary>
        /// 设备类型  A安卓手机B安卓平板C苹果手机D苹果平板 E Windows手机 F Windows平板	W:web网页		
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
    }
}
