using iMotto.Events;
using iMotto.Data.Entities.Models;

namespace iMotto.Cache
{
    public interface IVerifyCodeCache:IEventConsumer<SendVerifyCodeEvent>
    {
        VerifyCode PeekVerifyCodeViaMobile(string mobile);

        VerifyCode PopVerifyCodeViaMobile(string mobile);
    }
}
