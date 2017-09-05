using iMotto.Data.Entities;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IBanRepo:IRepository
    {
        Task<int> AddBanAsync(Ban ban, bool removeFollow, bool resetMutual);

        Task<int> RemoveBanAsync(Ban ban);
    }
}
