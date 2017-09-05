using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class TagCollection
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public int CID { get; set; }

        public string CollectionTitle { get; set; }

        public double CollectionScore { get; set; }
    }
}
