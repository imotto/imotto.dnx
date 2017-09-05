using System;

namespace iMotto.Data.Entities
{
    public class BillRecord
    {
        public long ID { get; set; }

        /// <summary>
        /// 变更类型 0：微言收益结算，1：礼品兑换
        /// </summary>
        public int ChangeType { get; set; }

        public int ChangeAmount { get; set; }

        public DateTime ChangeTime { get; set; }

        public string Summary { get; set; }
    }
}
