using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Collection
    {
        public long ID { get; set; }

        public double Score { get; set; }

        public string UID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Mottos { get; set; }

        public int Loves { get; set; }

        public DateTime CreateTime { get; set; }

        public string Tags { get; set; }
    }
}
