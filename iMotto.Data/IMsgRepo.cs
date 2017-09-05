using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iMotto.Data.Entities;

namespace iMotto.Data
{
    public interface IMsgRepo : IRepository
    {
        Task<int> SendMsgAsync(string uid, string tuid, string content, DateTime now);
        Task<List<Talk>> GetTalkMsgsAsync(string uid, string withUid, long start, int take);
        Task<List<RecentTalk>> GetRecentTalksAsync(string uid, int pIndex, int pSize);
        Task<List<Notice>> GetNoticesAsync(string userId, int pIndex, int pSize);
        
        Task<int> SetNoticeReadAsync(long iD, string userId);
    }
}
