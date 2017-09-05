using System;

namespace iMotto.Data.Entities
{
    public class Follow
    {
        public string SUID { get; set; }
        public string TUID { get; set; }
        public bool IsMutual { get; set; }

        public DateTime FollowTime { get; set; }
        public string SUname { get; set; }
    }


}
