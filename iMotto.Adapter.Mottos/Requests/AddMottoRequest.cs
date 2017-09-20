using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class AddMottoRequest : AuthedRequest
    {
        public string Content { get; set;}
        public int RID { get; set;}
    }
}
