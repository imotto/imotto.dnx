namespace iMotto.Events
{
    public class UpdateSexEvent:IEvent
    {
        public string UID { get; set; }

        public int Sex { get; set; }
    }
}
