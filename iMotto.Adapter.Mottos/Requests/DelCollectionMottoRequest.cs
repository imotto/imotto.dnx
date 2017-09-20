using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class DelCollectionMottoRequest : AuthedRequest
    {
        public long MID { get; set;}
        public long CID { get; set;}
    }
}
