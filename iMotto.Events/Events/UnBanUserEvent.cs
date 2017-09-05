namespace iMotto.Events
{
    public class UnBanUserEvent:IEvent
    {
        public string SUID { get; set; }

        public string TUID { get; set; }
    }
}
