using iMotto.Adapter.Users.Requests;
using iMotto.Adapter.Users.Results;
using iMotto.Common;
using iMotto.Data;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class UserLoginHandler : BaseHandler<UserLoginRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public UserLoginHandler(IEventPublisher eventPublisher, IUserRepo userRepo)
        {
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(UserLoginRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Mobile) ||
                string.IsNullOrWhiteSpace(reqObj.Password))
                return new UserLoginResult
                {
                    State = HandleStates.InvalidUserNameOrPassword,
                    Msg = "用户名或密码不正确"
                };

            var user = await _userRepo.GetByPhoneNumberAsync(reqObj.Mobile);
            
            if (user != null && user.Password.Equals(Utils.HashPassword(reqObj.Password)))
            {
                var usertoken = Utils.GenId();

                _eventPublisher.Publish(new UserLoginEvent {
                    UserId = user.Id,
                    Signature = reqObj.Sign,
                    Token =usertoken
                });

                _eventPublisher.Publish(new LoadUserInfoEvent
                {
                    UserInfo = user
                });
                

                return new UserLoginResult
                {
                    State = HandleStates.Success,
                    UserId = user.Id,
                    UserName = user.DisplayName ?? user.UserName,
                    UserToken = usertoken,
                    UserThumb = user.Thumb
                };
            }

            return new UserLoginResult
            {
                State = HandleStates.InvalidUserNameOrPassword,
                Msg = "用户名或密码不正确"
            };
        }
    }
}
