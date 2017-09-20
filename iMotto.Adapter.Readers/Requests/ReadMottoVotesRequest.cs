using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Requests
{
    class ReadMottoVotesRequest : PagedHandleRequest
    {
        public long MID { get; set; }
    }
}
