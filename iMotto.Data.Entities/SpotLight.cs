namespace iMotto.Data.Entities
{
    public class SpotLight
    {
        public int TheMonth { get; set; }
        /// <summary>
        /// 1、上月排行榜出炉，获奖用户公布  2、公布当月奖品设置  3、大事件
        /// </summary>
        public int Type { get; set; }

        public string Img { get; set; }

        public string Info { get; set; }
    }
}
