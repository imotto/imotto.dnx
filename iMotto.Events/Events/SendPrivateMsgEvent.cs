using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Events
{
    public class SendPrivateMsgEvent:IEvent
    {
        public string SUID { get; set; }
        public string TUID { get; set; }
        public string Msg { get; set; }
    }
}
