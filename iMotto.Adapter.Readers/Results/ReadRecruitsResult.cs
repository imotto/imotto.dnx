using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Results
{
    class ReadRecruitsResult:HandleResult
    {
        public List<Recruit> Warming { get; set; }

        public List<Recruit> Collecting { get; set; }

        public List<Recruit> Threshing { get; set; }

        public List<Recruit> Finalizing { get; set; }
    }
}
