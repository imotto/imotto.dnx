using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Requests
{
    class ReadUserCollectionsRequest : PagedHandleRequest
    {
        public string UID { get; set; }
        public long MID { get; set; }
    }
}
