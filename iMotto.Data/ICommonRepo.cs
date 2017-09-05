using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotto.Data.Entities;

namespace iMotto.Data
{
    public interface ICommonRepo : IRepository
    {
        /// <summary>
        /// 获取更新
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<UpdateInfo> TryUpdateAsync(string sign, string type, int version);

        /// <summary>
        /// 注册设备
        /// </summary>
        /// <param name="info"></param>
        /// <param name="uinfo">更新信息</param>
        /// <returns></returns>
        int RegisterDevice(DeviceReg info, out UpdateInfo uinfo);
    }
}
