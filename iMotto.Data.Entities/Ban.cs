using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Ban
    {
        public string SUID { get; set; }
        public string TUID { get; set; }
        public DateTime BanTime { get; set; }

    }
}
