using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    /// <summary>
    /// 在线用户状态相关缓存
    /// </summary>
    public interface IOnlineUserCache:IEventConsumer<UserLoginEvent>
    {
        /// <summary>
        /// 使用设备签名查询当前登录此设备的用户信息,返回一个元组.
        /// Item1: UserId
        /// Item2: UserToken
        /// </summary>
        /// <param name="sign"></param>
        /// <returns>Tuple&lt;string,string&gt; Item1:UserId, Item2:UserToken</returns>
        Tuple<string, string> GetOnlineUserViaSignature(string sign);

        /// <summary>
        /// 判断用户是否具有指定角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool HasUserInRole(string userId, string role);

        void Logout(string sign);
    }
}
