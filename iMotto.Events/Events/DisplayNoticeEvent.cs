using iMotto.Data.Entities.Models;
namespace iMotto.Events
{
    public class DisplayNoticeEvent:IEvent
    {
        public DisplayNotice DisplayNotice { get; set; }

        public int TheMonth { get; set; }

        public string Sign { get; set; }
    }
}
