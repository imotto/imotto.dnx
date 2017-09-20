using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class UnloveMottoRequest : AuthedRequest
    {
        public long MID { get; set; }

        public int TheDay { get; set; }
    }
}
