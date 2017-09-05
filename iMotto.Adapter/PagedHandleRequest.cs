using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Adapter
{
    /// <summary>
    /// 分页请求
    /// </summary>
    public class PagedHandleRequest : HandleRequest
    {
        private int pindex;
        private int psize;

        /// <summary>
        /// 页码
        /// </summary>
        public int PIndex
        {
            get { return pindex; }
            set
            {
                if (value <= 0)
                    pindex = 1;
                else
                    pindex = value;
            }
        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PSize
        {
            get
            {
                return psize;
            }
            set
            {
                if (value <= 0 || value >= 500)
                {
                    psize = 30;
                }
                else
                {
                    psize = value;
                }
            }
        }
    }
}
