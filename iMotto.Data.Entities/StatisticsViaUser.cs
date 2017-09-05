using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class StatisticsViaUser
    {
        public string UID { get; set; }

        public int Mottos { get; set; }

        public int Recruits { get; set; }

        public int Collections { get; set; }

        public int Reviews { get; set; }

        public int Votes { get; set; }

        public int LovedMottos { get; set; }

        public int LovedCollections { get; set; }

        public int Revenue { get; set; }

        public int Balance { get; set; }

        public int Follows { get; set; }

        public int Followers { get; set; }

        public int Bans { get; set; }
    }
}
