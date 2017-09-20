using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class LoveCollectionRequest : AuthedRequest
    {
        public long CID { get; set;}
    }
}
