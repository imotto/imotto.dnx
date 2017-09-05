using System;

namespace iMotto.Data.Entities
{
    public class Talk
    {
        public long ID { get; set; }

        public string WithUID { get; set; }
        
        /// <summary>
        /// 消息方向: 0：发送，1：接收
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        public DateTime SendTime { get; set; }
    }
}
