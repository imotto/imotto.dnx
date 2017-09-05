namespace iMotto.Events
{
    public class UnLoveUserEvent:IEvent
    {
        public string SUID { get; set; }

        public string TUID { get; set; }
    }
}
