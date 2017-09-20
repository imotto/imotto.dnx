using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class AddReviewRequest : AuthedRequest
    {
        public int TheDay { get; set; }
        public string Content { get; set;}
        public long MID { get; set;}
    }
}
