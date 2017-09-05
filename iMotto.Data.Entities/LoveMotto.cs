using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class LoveMotto
    {
        public string UID { get; set; }

        public long MID { get; set; }

        public int TheDay { get; set; }

        public DateTime LoveTime { get; set; }
    }
}
