using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    class ExpirableObj<T>
    {
        public DateTime ExpireTime { get; set; }

        public T Obj { get; set; }
    }
}
