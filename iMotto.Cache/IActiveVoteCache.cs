using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    [Obsolete("使用IOnlineUserCache",true)]
    public interface IActiveVoteCache:IEventConsumer<CreateVoteEvent>
    {
        bool HasVoted(string uid, long mid, Func<string, List<Vote>> voteLoader = null);
    }
}
