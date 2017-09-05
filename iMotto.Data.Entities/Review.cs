using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Review
    {
        public long MID { get; set; }

        public long ID { get; set; }

        public string UID { get; set; }

        public string Content { get; set; }

        public int Up { get; set; }

        public int Down { get; set; }

        public DateTime AddTime { get; set; }
    }
}
