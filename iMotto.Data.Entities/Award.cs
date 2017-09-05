using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Award
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Img { get; set; }
        /// <summary>
        /// 奖品状态：0:评估中，1：已结束,
        /// </summary>
        public int Status { get; set; }
        public int Amount { get; set; }

        /// <summary>
        /// 0:未获得，1：待确认地址，2：待发放，3：已发放，4：已签收
        /// </summary>
        public int GainStatus { get; set; }
    }
}
