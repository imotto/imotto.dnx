using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Events
{
    public class UserLoginEvent:IEvent
    {
        public string UserId { get; set; }

        public string Signature { get; set; }

        public string Token { get; set; }
    }
}
