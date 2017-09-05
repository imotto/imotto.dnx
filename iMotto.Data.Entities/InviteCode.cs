using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class InviteCode
    {
        /// <summary>
        /// 有效
        /// </summary>
        public const int STATUS_NEW = 0;
        /// <summary>
        /// 保留
        /// </summary>
        public const int STATUS_RESERVED = 1;
        /// <summary>
        /// 已使用
        /// </summary>
        public const int STATUS_USED = 2;
        /// <summary>
        /// 已过期
        /// </summary>
        public const int STATUS_EXPIRED = 3;
        /// <summary>
        /// 微信
        /// </summary>
        public const int SOURCE_WECHAT = 0;
        /// <summary>
        /// 用户创建
        /// </summary>
        public const int SOURCE_USER = 1;        
        /// <summary>
        /// 特邀VIP
        /// </summary>
        public const int SOURCE_VIP = 2;


        public string GenUId { get; set; }

        public string UseUId { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }

        public int Source { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpireTime { get; set; }

        public DateTime UseTime { get; set; }

        
    }
}
