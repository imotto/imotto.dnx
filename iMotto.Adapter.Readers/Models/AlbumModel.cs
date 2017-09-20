using iMotto.Data.Entities;

namespace iMotto.Adapter.Readers
{
    class AlbumModel:Collection
    {
        public string UserName { get; set; }

        public string UserThumb { get; set; }

        /// <summary>
        /// 喜欢状态，NotYet=0, Loved=1
        /// </summary>
        public int Loved { get; set; }

        /// <summary>
        /// 包含请求时提供的MID相关的Motto  No=0, Other=MID
        /// </summary>
        public long ContainsMID { get; set; }
    }
}
