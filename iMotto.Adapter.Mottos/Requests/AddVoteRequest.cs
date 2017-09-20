using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class AddVoteRequest : AuthedRequest
    {
        public long MID { get; set;}
        public int TheDay { get; set; }
        
        /// <summary>
        /// 0:中立, 1:支持, -1:反对
        /// </summary>
        public int Support { get; set;}
    }
}
