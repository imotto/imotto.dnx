using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Requests
{
    class ReadUserRecruitsRequest:AuthedRequest
    {
        public int PIndex { get; set; }
        public int PSize { get; set; }
    }
}
