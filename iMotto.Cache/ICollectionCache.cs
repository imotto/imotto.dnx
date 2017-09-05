using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    public interface ICollectionCache : IEventConsumer<CreateCollectionEvent>,
        IEventConsumer<LoveCollectionEvent>,
        IEventConsumer<UnLoveCollectionEvent>,
        IEventConsumer<CollectMottoEvent>,
        IEventConsumer<UnCollectMottoEvent>
    {
        Task<List<long>> WhichHasContainsMID(long mID, IEnumerable<long> cids);
    }
}
