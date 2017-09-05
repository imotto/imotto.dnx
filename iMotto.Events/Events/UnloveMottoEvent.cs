using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class UnloveMottoEvent:IEvent
    {
        public LoveMotto LoveMotto { get; set; }
    }
}
