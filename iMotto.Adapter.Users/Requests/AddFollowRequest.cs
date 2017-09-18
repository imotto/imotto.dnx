using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Requests
{
    class AddFollowRequest : AuthedRequest
    {
        public string TargetUId { get; set; }
    }
}
