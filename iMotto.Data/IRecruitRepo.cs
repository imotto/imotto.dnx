using iMotto.Data.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace iMotto.Data
{
    public interface IRecruitRepo:IRepository
    {
        Task<int> AddRecruitApplyAsync(RecruitA ra);

        Task<int> RemoveRecruitApplyAsync(RecruitA ra);

        Task<int> AddRecruitAsync(Recruit r);

        Task<int> AddRecruitWinnerAsync(RecruitWinner rw);

        Task<List<Recruit>> GetRecruitsAsync(int start, int end);

        Task<List<Recruit>> GetRecruitsByUserAsync(string userId, int pIndex, int pSize);
    }
}
