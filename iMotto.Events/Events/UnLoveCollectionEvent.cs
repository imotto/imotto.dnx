using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class UnLoveCollectionEvent:IEvent
    {
        public LoveCollection LoveCollection { get; set; }
    }
}
