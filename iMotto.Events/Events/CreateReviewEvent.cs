using iMotto.Data.Entities;

namespace iMotto.Events
{
    public class CreateReviewEvent:IEvent
    {
        public Review Review { get; set; }
        public int TheDay { get; set; }
    }
}
