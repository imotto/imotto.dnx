using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class UpdateInfo
    {
        public string DeviceType { get; set; }

        public string DisplayVersion { get; set; }

        public int DigitVersion { get; set; }

        public DateTime PubDate { get; set; }

        public string PubDesc { get; set; }

        public string Url { get; set; }

        public bool ForceUpdate { get; set; }

    }
}
