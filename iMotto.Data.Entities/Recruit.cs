using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class Recruit
    {
        /// <summary>
        /// yyyyMMddxx
        /// </summary>
        public int ID { get; set; }

        public string UID { get; set; }

        /// <summary>
        /// max 255
        /// </summary>
        public string Title { get; set; }

        public int Drafts { get; set; }

        public int Involved { get; set; }

        public int Actions { get; set; }

        public int Cost { get; set; }

        public int Times { get; set; }

        public DateTime Start { get; set; }

        public DateTime StopTime { get; set; }

        public string Description { get; set; }

        public int Charges { get; set; }
    }

    public class RecruitA
    {
        public int ID { get; set; }

        public string UID { get; set; }

        public int Times { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime StartTime { get; set; }

        public string Description{get;set;}

        public string Result { get; set; }

        public string Title { get; set; }
    }
}
