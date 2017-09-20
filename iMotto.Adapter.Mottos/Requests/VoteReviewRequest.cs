using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class VoteReviewRequest : AuthedRequest
    {
        public long MID { get; set;}
        public long RID { get; set;}
        public int Support { get; set;}
    }
}
