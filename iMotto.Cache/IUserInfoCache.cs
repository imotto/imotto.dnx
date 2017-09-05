using iMotto.Data.Entities;
using iMotto.Events;
using iMotto.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    public interface IUserInfoCache:IEventConsumer<LoadUserInfoEvent>,        
        IEventConsumer<CreateMottoEvent>, 
        IEventConsumer<CreateVoteEvent>,
        IEventConsumer<LoveMottoEvent>,
        IEventConsumer<UnloveMottoEvent>,
        IEventConsumer<CreateReviewEvent>,
        IEventConsumer<RemoveReviewEvent>,
        IEventConsumer<CreateCollectionEvent>,
        IEventConsumer<LoveCollectionEvent>,
        IEventConsumer<UnLoveCollectionEvent>,
        IEventConsumer<CollectMottoEvent>,
        IEventConsumer<UnCollectMottoEvent>,
        IEventConsumer<UpdateUserNameEvent>,
        IEventConsumer<UpdateUserThumbEvent>,
        IEventConsumer<LoveUserEvent>,
        IEventConsumer<UnLoveUserEvent>,
        IEventConsumer<BanUserEvent>,
        IEventConsumer<UnBanUserEvent>,
        IEventConsumer<UpdateSexEvent>,
        IEventConsumer<SendPrivateMsgEvent>
    {
        User GetUserById(string uid);

        List<long> GetUserCollectionIds(string uID, int pIndex, int pSize);

        Task<List<long>> GetCollectedMottoIds(string uid, IEnumerable<long> mids);

        
        Task<List<long>> GetLovedMottoIds(string uid, IEnumerable<long> mids);

        Task<Dictionary<long,int>> GetVotedStates(string uid, IEnumerable<long> mids);

        Task<List<long>> GetLovedCollectionIds(string uid, IEnumerable<long> cids);

        Task<List<long>> GetReviewedMottoIds(string uid, IEnumerable<long> mids);

        Task<Dictionary<string, UserRelation>> GetRelations(string suid, IEnumerable<string> tuids);
        
        Task<Dictionary<string, bool>> HasBans(string suid, IEnumerable<string> tuids);

        Task<Dictionary<string, bool>> IsFollowers(string suid, IEnumerable<string> tuids);

        /// <summary>
        /// 验证用户是否已为指定Motto投过票
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="mid"></param>
        /// <returns>投票状态 NotYet = 9, Supported = 1, Opposed = -1, Middle=0</returns>
        int HasVoted(string uid, long mid);


        bool HasReviewed(string uid, long iD);

        /// <summary>
        /// 验证用户是否已将指定Motto置为喜欢
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        bool HasLovedMotto(string uid, long mid);

        /// <summary>
        /// 验证用户是否已收藏指定的Motto
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="iD"></param>
        /// <returns></returns>
        bool HasCollectedMotto(string uid, long iD);
        

        //Task<Tuple<long?, long?>> GetUserRank(string uid, int theday, int yesterday);
        
        //List<UserScore> GetUserRange(int theday, int yesterday, int start, int stop);

        //Task TryFillUserInfo(List<UserScore> users);

        Task<UserRelation> GetRelation(string userId, string targetUId);
        
    }
}
