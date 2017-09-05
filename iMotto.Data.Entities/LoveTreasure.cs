using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class LoveCollection
    {
        public long ID { get; set; }

        public string UID { get; set; }

        public DateTime LoveTime { get; set; }

        public long CID { get; set; }

        public string CollectionTitle { get; set; }
    }
}
