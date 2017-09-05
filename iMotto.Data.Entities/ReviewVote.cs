using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class ReviewVote
    {
        public long ID { get; set; }

        public long MID { get; set; }

        public long ReviewID { get; set; }

        public string UID { get; set; }

        public int Support { get; set; }

        public int Oppose { get;set;}


        public DateTime VoteTime { get; set; }
    }
}
