using System;

namespace iMotto.Data.Entities
{
    public class Notice
    {
        public long ID { get; set; }

        public string UID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Type { get; set; }

        public DateTime SendTime { get; set; }

        public int State { get; set; }

        public string TargetID { get; set; }

        public string TargetInfo { get; set; }
    }
}
