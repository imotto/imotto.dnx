using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Requests
{
    class ReadStatisticsRequest : HandleRequest
    {
        public int TheMonth { get; set; }
    }
}
