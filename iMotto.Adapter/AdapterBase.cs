using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iMotto.Adapter
{
    public class AdapterBase : IAdapter
    {
        public virtual IHandler GetHandler(string code)
        {
            return new DefaultHandler();
        }
    }

    class DefaultHandler : BaseHandler<HandleRequest>
    {
        protected override Task<HandleResult> HandleCoreAsync(HandleRequest reqObj)
        {
            return Task.FromResult(new HandleResult
            {
                Code = reqObj.Code,
                State = HandleStates.InvalidData,
                Msg = "UNKNOWN HANDLER"
            });
        }
    }
}
