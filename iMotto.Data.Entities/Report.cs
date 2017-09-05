using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Report
    {
        public int ID { get; set; }

        public string UID { get; set; }

        public int Type { get; set; }

        public string TargetID { get; set; }

        public string Reason { get; set; }

        /// <summary>
        /// 0：未处理，1：已处理
        /// </summary>
        public int Status { get; set; }

        public string ProcessUID { get; set; }

        public string Result { get; set; }

        public DateTime ReportTime { get; set; }

        public DateTime ProcessTime { get; set; }
    }
}
