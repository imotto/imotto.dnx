using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Cache
{
    public interface IEvaluatingMottoCache:IEventConsumer<CreateMottoEvent>,
        IEventConsumer<CreateVoteEvent>,
        IEventConsumer<LoveMottoEvent>,
        IEventConsumer<UnloveMottoEvent>,
        IEventConsumer<CreateReviewEvent>,
        IEventConsumer<RemoveReviewEvent>
    {
        Motto FindMotto(int theDay, long mid);

        [Obsolete]
        List<Motto> GetMottos(int theDay, Func<DateTime, int, int, List<Motto>> mottoLoader = null);

        Task<Motto[]> GetMottos(int theDay, int page, int pSize);
    }
}
