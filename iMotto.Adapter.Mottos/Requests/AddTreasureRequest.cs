using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Mottos.Requests
{
    class AddTreasureRequest : AuthedRequest
    {
        public string Summary { get; set;}
        public string Tags { get; set;}
        public string Title { get; set;}
    }
}
