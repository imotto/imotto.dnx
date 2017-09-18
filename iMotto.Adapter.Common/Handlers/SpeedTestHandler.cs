using System.Threading.Tasks;

namespace iMotto.Adapter.Common
{
    /// <summary>
    /// 测速请求处理器
    /// </summary>
    class SpeedTestHandler:BaseHandler<HandleRequest>
    {
        public SpeedTestHandler()
        {
            NeedSign = false;
        }
    
        protected override Task<HandleResult> HandleCoreAsync(HandleRequest reqObj)
        {
            return Task.FromResult(new HandleResult
            {
                State = HandleStates.Success,
                Msg = string.Empty
            });
        }
    }
}
