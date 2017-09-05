using iMotto.Data.Entities.Models;

namespace iMotto.Events
{
    public class SendVerifyCodeEvent:IEvent
    {
        public VerifyCode VerifyCode { get; set; }
    }
}
