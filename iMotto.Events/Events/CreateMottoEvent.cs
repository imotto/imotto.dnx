using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class CreateMottoEvent:IEvent
    {
        public Motto Motto { get; set; }
    }
}
