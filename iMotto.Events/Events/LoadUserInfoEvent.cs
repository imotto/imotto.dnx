using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Events
{
    public class LoadUserInfoEvent:IEvent
    {
        public User UserInfo { get; set; }
    }
}
