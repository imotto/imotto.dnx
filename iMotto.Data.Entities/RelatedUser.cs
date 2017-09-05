using System;

namespace iMotto.Data.Entities
{
    public class RelatedUser
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Thumb { get; set; }

        public int Sex { get; set; }

        public bool IsMutual { get; set; }

        public int Mottos { get; set; }

        public int Revenue { get; set; }

        public int Rank { get; set; }

        /// <summary>
        /// 与当前请求用户（Source）的关系状态, 此用户为Target,  0:None, 1: SLoveT, 2: TLoveS, 4: SBanT,
        /// 赋值时请使用枚举 RelationState 的运算结果表示其它关系
        /// 互相喜欢: RelationState.SLoveT | RelationState.TLoveS (数字值等于 3 )
        /// 尴尬关系: RelationState.TLoveS | RelationState.SBanT (数字值等于 6)
        /// 被拉黑的人是看不到被拉黑的状态的。
        /// </summary>
        public int RelationState { get; set; }

        public int Follows { get; set; }

        public int Followers { get; set; }

        public DateTime FollowTime { get; set; }
    }
}
