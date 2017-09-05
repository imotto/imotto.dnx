namespace iMotto.Events
{
    public class UpdateUserNameEvent:IEvent
    {
        public string UID { get; set; }

        public string UserName { get; set; }
    }
}
