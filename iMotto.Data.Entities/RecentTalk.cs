using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Data.Entities
{
    public class RecentTalk
    {
        public long ID { get; set; }
        
        public string WithUID { get; set; }

        public string UserName { get; set; }

        public string UserThumb { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 消息方向 0：发送，1：接收
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// 未读消息数
        /// </summary>
        public int Msgs { get; set; }

        public DateTime LastTime { get; set; }
    }


    public enum TalkDirection
    {
        Send,
        Receive
    }

    public enum TalkStatus
    {
        New,
        Readed
    }
}
