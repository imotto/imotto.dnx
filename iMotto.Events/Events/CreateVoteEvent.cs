using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class CreateVoteEvent:IEvent
    {
        //public Vote Vote { get; set; }

        public string UID { get; set; }
        public int TheDay { get; set; }
        public long MID { get; set; }
        public int Vote { get; set; }
    }
}
