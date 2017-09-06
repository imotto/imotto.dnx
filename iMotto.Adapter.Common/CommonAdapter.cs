using System;

namespace iMotto.Adapter.Common
{
    public class CommonAdapter : AdapterBase
    {
        private readonly IServiceProvider _serviceProvider;
        public CommonAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static string Code => Constants.ADAPTER_CODE;

        public override IHandler GetHandler(string code)
        {
            IHandler handler;

            switch (code)
            {
                case Constants.HANDLER_REGISTER_DEVICE:
                    handler = _serviceProvider.GetHandler<RegisterDeviceHandler>();
                    break;
                case Constants.HANDLER_UPDATE:
                    handler = _serviceProvider.GetHandler<UpdateHandler>();
                    break;
                case Constants.HANDLER_VERIFYCODE:
                    handler = _serviceProvider.GetHandler<VerifyCodeHandler>();
                    break;
                case Constants.HANDLER_SPEED_TEST:
                    handler = _serviceProvider.GetHandler<SpeedTestHandler>();
                    break;
                default:
                    handler = base.GetHandler(code);
                    break;
            }

            return handler;
        }
    }
}
