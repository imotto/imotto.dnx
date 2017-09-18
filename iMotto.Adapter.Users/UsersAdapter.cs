using iMotto.Adapter.Users.Handlers;
using System;

namespace iMotto.Adapter.Users
{
    public class UsersAdapter : AdapterBase
    {
        private readonly IServiceProvider _serviceProvider;

        public UsersAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IHandler GetHandler(string code)
        {
            IHandler handler;
            switch (code)
            {
                case Constants.HANDLER_ADD_BAN: handler = _serviceProvider.GetHandler<AddBanHandler>(); break;
                case Constants.HANDLER_ADD_FOLLOW: handler = _serviceProvider.GetHandler<AddFollowHandler>(); break;
                case Constants.HANDLER_ADD_USER: handler = _serviceProvider.GetHandler<AddUserHandler>(); break;
                case Constants.HANDLER_DEL_BAN: handler = _serviceProvider.GetHandler<DelBanHandler>(); break;
                case Constants.HANDLER_DEL_FOLLOW: handler = _serviceProvider.GetHandler<DelFollowHandler>(); break;
                case Constants.HANDLER_USER_LOGIN: handler = _serviceProvider.GetHandler<UserLoginHandler>(); break;
                case Constants.HANDLER_USER_LOGOUT: handler = _serviceProvider.GetHandler<UserLogoutHandler>(); break;
                case Constants.HANDLER_MODIFY_USERNAME: handler = _serviceProvider.GetHandler<ModifyUserNameHandler>(); break;
                case Constants.HANDLER_MODIFY_THUMB: handler = _serviceProvider.GetHandler<ModifyThumbHandler>(); break;
                case Constants.HANDLER_MODIFY_PASSWORD: handler = _serviceProvider.GetHandler<ModifyPasswordHandler>(); break;
                case Constants.HANDLER_MODIFY_SEX: handler = _serviceProvider.GetHandler<ModifySexHandler>(); break;
                case Constants.HANDLER_SEND_MSG: handler = _serviceProvider.GetHandler<SendMsgHandler>(); break;
                case Constants.HANDLER_ADD_REPORT: handler = _serviceProvider.GetHandler<AddReportHandler>(); break;
                case Constants.HANDLER_SET_NOTICE_READ: handler = _serviceProvider.GetHandler<SetNoticeReadHandler>(); break;
                case Constants.HANDLER_RESET_PASSWORD: handler = _serviceProvider.GetHandler<ResetPasswordHandler>(); break;
                case Constants.HANDLER_ADD_ADDRESS: handler = _serviceProvider.GetHandler<AddAddressHandler>(); break;
                case Constants.HANDLER_ADD_REL_ACCOUNT: handler = _serviceProvider.GetHandler<AddRelAccountHandler>(); break;
                case Constants.HANDLER_SET_DEFAULT_ADDRESS: handler = _serviceProvider.GetHandler<SetDefaultAddressHandler>(); break;
                case Constants.HANDLER_PREPARE_EXCHANGE: handler = _serviceProvider.GetHandler<PrepareExchangeHandler>(); break;
                case Constants.HANDLER_DO_EXCHANGE: handler = _serviceProvider.GetHandler<ExchangeHandler>(); break;
                case Constants.HANDLER_RECEIVE_GIFT: handler = _serviceProvider.GetHandler<ReceiveGiftHandler>(); break;
                case Constants.HANDLER_REVIEW_GIFT: handler = _serviceProvider.GetHandler<ReviewGiftHandler>(); break;
                case Constants.HANDLER_SET_AWARD_ADDRESS: handler = _serviceProvider.GetHandler<SetAwardAddressHandler>();break;
                case Constants.HANDLER_RECEIVE_AWARD:handler = _serviceProvider.GetHandler<ReceiveAwardHandler>();break;

                default: handler = base.GetHandler(code); break;
            }

            return handler;
        }
    }
}
