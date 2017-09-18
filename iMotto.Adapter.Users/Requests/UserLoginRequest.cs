using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Requests
{
    class UserLoginRequest : HandleRequest
    {
        public string Password { get; set; }
        public string Mobile { get; set; }
    }
}
