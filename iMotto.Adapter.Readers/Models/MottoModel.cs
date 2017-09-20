using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers
{
    public class MottoModel:Motto
    {
        public string UserName { get; set; }

        public string UserThumb { get; set; }

        /// <summary>
        /// 状态 Evaluating = 0, Permanent = 1;
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 是否已喜欢 Nofeeling = 0, Love = 1
        /// </summary>
        public int Loved { get; set; }

        /// <summary>
        /// 收藏状态 NotYet = 0, Collected = 1
        /// </summary>
        public int Collected { get; set; }
        /// <summary>
        /// 投票状态  NotYet = 9, Middle = 0   Supported = 1, Opposed = -1
        /// </summary>
        public int Vote { get; set; }

        /// <summary>
        /// 评论状态 NotYet = 0 , Reviewed = 1
        /// </summary>
        public int Reviewed { get; set; }
    }
}
