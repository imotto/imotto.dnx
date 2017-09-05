using iMotto.Data.Entities;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IFollowRepo : IRepository
    {
        /// <summary>
        /// 添加喜欢用户
        /// </summary>
        /// <param name="follow"></param>
        /// <param name="removeBan">是否需要移除之前的拉黑关系</param>
        /// <returns></returns>
        Task<int> AddFollowAsync(Follow follow, bool removeBan);

        Task<int> RemoveFollowAsync(Follow follow);
    }
}