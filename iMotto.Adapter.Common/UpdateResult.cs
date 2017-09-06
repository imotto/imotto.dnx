using System;

namespace iMotto.Adapter.Common
{
    class UpdateResult:HandleResult
    {
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public string UpdateFlag { get; set; }
        public string CurVer { get; set; }
        public int CurVerCode { get; set; }
        public string Url { get; set; }
        public string UpdateDes { get; set; }

        public DateTime ReleaseTime { get; set; }
    }
}
