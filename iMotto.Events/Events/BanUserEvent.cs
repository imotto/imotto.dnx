namespace iMotto.Events
{
    public class BanUserEvent:IEvent
    {
        public string SUID { get; set; }

        public string TUID { get; set; }
    }
}
