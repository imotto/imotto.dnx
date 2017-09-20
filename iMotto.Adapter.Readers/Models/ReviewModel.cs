using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers
{
    public class ReviewModel:Review
    {
        public string UserName { get; set; }

        public string UserThumb { get; set; }

        /// <summary>
        /// 支持状态 NotYet = 0, Supported = 1
        /// </summary>
        public int Supported { get; set; }
    }
}
