using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class CollectMottoEvent:IEvent
    {
        public string UID { get; set; }
        public CollectionMotto CollectionMotto { get; set; }
    }
}
