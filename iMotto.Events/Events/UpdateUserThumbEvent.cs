namespace iMotto.Events
{
    public class UpdateUserThumbEvent:IEvent
    {
        public string UID { get; set; }

        public string Thumb { get; set; }
    }
}
