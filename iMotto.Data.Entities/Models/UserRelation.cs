namespace iMotto.Data.Entities.Models
{
    public class UserRelation
    {
        /// <summary>
        /// 当前用户是否已将目标用户拉黑
        /// </summary>
        public bool SBanT { get; set; }

        /// <summary>
        /// 当前用户是否已喜欢目标用户
        /// </summary>
        public bool SLoveT { get; set; }

        /// <summary>
        /// 目标用户是否已喜欢当前用户
        /// </summary>
        public bool TLoveS { get; set; }
    }
}
