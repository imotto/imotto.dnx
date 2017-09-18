using iMotto.Adapter.Users.Requests;
using iMotto.Common;
using iMotto.Data;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ModifyUserNameHandler : BaseHandler<ModifyUserNameRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ModifyUserNameHandler(IEventPublisher eventPublisher, IUserRepo userRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ModifyUserNameRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.UserName))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "用户名称不能为空"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.UserName))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "用户名中包含敏感词语，请修正"
                };
            }

            var rowAffected = await _userRepo.UpdateUserNameAsync(reqObj.UserId, reqObj.UserName);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UpdateUserNameEvent {
                    UID = reqObj.UserId,
                    UserName = reqObj.UserName
                });

                return new HandleResult {
                    State = HandleStates.Success,
                    Msg="用户名称修改成功"
                };
            }

            return new HandleResult
            {
                State = HandleStates.NoDataFound,
                Msg = "未找到要更新的用户信息"
            };
        }
    }
}
