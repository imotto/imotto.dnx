using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    /// <summary>
    /// 礼品发货结果
    /// </summary>
    public enum DeliverResult
    {
        /// <summary>
        /// 发货成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 赞助商与当前操作用户不符
        /// </summary>
        BadVendor = 1,
        /// <summary>
        /// 错误的兑换代码
        /// </summary>
        BadExcode = 2,
        /// <summary>
        /// 发货状态不符
        /// </summary>
        StatusError = 3
    }
}
