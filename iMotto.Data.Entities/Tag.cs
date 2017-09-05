using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Tag
    {
        public int ID { get; set; }

        /// <summary>
        /// max 7
        /// </summary>
        public string Name { get; set; }

        public int Collections { get; set; }

    }
}
