using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace iMotto.Data
{
    public interface IMottoRepo : IRepository
    {
        Task<int> AddMottoAsync(Motto m);

        Task<int> AddLoveMottoAsync(LoveMotto lm);

        Task<int> RemoveLoveMottoAsync(LoveMotto lm);

        Task<int> AddReviewAsync(Review review);

        Task<int> RemoveReviewAsync(Review review);

        Task<int> AddReviewVoteAsync(ReviewVote rv);

        Task<int> AddVoteAsync(Vote v);
        
        List<long> GetUserLovedMottoIds(string uid, int theDay);

        List<Vote> GetUserVotes(string uid, int theDay);

        Task<List<Motto>> GetUserLovedMottosAsync(string uID, int pIndex, int pSize);

        List<Motto> GetMottosBetween(long start, long end);

        Task<List<Motto>> GetMottosByCollectionAsync(long cID, int pIndex, int pSize);

        Task<List<Motto>> GetMottosByUserAsync(string uID, int pIndex, int pSize, bool skipEvaluatingMottos = false);

        Task<List<Vote>> GetVotesByMottoAsync(long mID, int pIndex, int pSize);

        Task<List<Motto>> GetMottosByRecruitAsync(int rID, int pIndex, int pSize);

        Task<List<Review>> GetReviewsByMottoId(long mID, int pIndex, int pSize);

        List<Motto> GetMottosByDay(DateTime theDay, int pIndex, int pSize);

        Task<List<Motto>> GetMottosByDayAsync(DateTime theDay, int pIndex, int pSize);

        Task<List<long>> GetVotedReviewIdsByUserAndMottoIdAsync(string uid, long mid);



    }
}
