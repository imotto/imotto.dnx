using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Requests
{
    class MofiyThumbRequest : AuthedRequest
    {
        public string Thumb { get; set; }
    }
}
