using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class LoveMottoEvent : IEvent
    {
        public LoveMotto LoveMotto { get; set; }
    }
}
