using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Requests
{
    class ReadCollectionMottosRequest:PagedHandleRequest
    {
        public long CID { get; set; }

        /// <summary>
        /// 是否当前创建的珍藏 Yes = 1 No=0
        /// </summary>
        public int IsMyAlbum { get; set; }
    }
}
