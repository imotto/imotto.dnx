using System;

namespace iMotto.Data.Entities
{
    public class Gift
    {
        public int ID { get; set;}
        /// <summary>
        /// 礼品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 赞助商名称
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 商家链接
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 兑换价格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 礼品数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int Available { get; set; }
        /// <summary>
        /// 礼品评分
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 兑换状态 0:正常,1:下架
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 已兑数量
        /// </summary>
        public int Sales { get; set; }
        /// <summary>
        /// 礼品类型 0:虚拟礼品，1：实物礼品
        /// </summary>
        public int GiftType { get; set; }
        /// <summary>
        /// 限制条件 0：每人限兑一次，1：每人每月限兑一次，2：无限制
        /// </summary>
        public int LimitType { get; set; }
        /// <summary>
        /// 兑换所需信息 0: 收货地址，其它为关联账户（对应关联账户表平台类型）：
        ///1、微信，2、支付宝，3、淘宝
        /// </summary>
        public int RequireInfo { get; set; }

        public int Reviews { get; set; }

        public string VendorUID { get; set; }
    }

    public class GiftExchange
    {
        public long ID { get; set; }
        public string UID { get; set; }
        public int GiftId { get; set; }
        public string GiftName { get; set; }
        public string Img { get; set; }
        public int Amount { get; set; }
        public int Total { get; set; }
        public DateTime ExchangeTime { get; set; }
        public int DeliverState { get; set; }
        public double Rate { get; set; }
        public string Review { get; set; }

        public string ExCode { get; set; }
        public string FailReason { get; set; }

        public int ReqInfoType { get; set; }
        public long ReqInfoId { get; set; }

    }
}
