using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class RecruitWinner
    {
        public int RID { get; set; }

        public int Ranking { get; set; }

        public long MID { get; set; }

        public string UID { get; set; }

        public string Content { get; set; }

        public double Score { get; set; }

        public int Up { get; set; }

        public int Down { get; set; }

        public DateTime AddTime { get; set; }

        public string Remark { get; set; }
    }
}
