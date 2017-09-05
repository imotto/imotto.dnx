using System;

namespace iMotto.Data.Entities
{
    public class User
    {
        public User()
        {
            Statistics = new StatisticsViaUser();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Thumb { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 用户排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 用户排名变化，负数为下降，正数为上升
        /// </summary>
        public int Change { get; set; }

        public int Sex { get; set; }

        /// <summary>
        /// 与当前请求用户（Source）的关系状态, 此用户为Target,  0:None, 1: SLoveT, 2: TLoveS, 4: SBanT,
        /// 赋值时请使用枚举 RelationState 的运算结果表示其它关系
        /// 互相喜欢: RelationState.SLoveT | RelationState.TLoveS (数字值等于 3 )
        /// 尴尬关系: RelationState.TLoveS | RelationState.SBanT (数字值等于 6)
        /// 被拉黑的人是看不到被拉黑的状态的。
        /// </summary>
        public int RelationState { get; set; }
        
        public StatisticsViaUser Statistics { get; set; }
    }

    /// <summary>
    /// 用于描述两个用户之间的关系
    /// </summary>
    [Flags]
    public enum RelationState
    {
        None = 0,
        SLoveT = 1,
        TLoveS = 2,
        SBanT = 4
    }
}
