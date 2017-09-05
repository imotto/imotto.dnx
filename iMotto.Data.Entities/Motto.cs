using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Motto
    {
        public long ID { get; set; }

        public double Score { get; set; }

        public string UID { get; set; }

        public int Up { get; set; }

        public int Down { get; set; }

        public int Reviews { get; set; }

        public int Loves { get; set; }

        public int RecruitID { get; set; }

        public string RecruitTitle { get; set; }

        public string Content { get; set; }

        public DateTime AddTime { get; set; }
    }
}
