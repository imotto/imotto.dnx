namespace iMotto.Events
{
    public class LoveUserEvent:IEvent
    {
        public string SUID { get; set; }

        public string TUID { get; set; }

    }
}
