using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ModifyThumbHandler : BaseHandler<MofiyThumbRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ModifyThumbHandler(IEventPublisher eventPublisher, IUserRepo userRepo) 
        {
            NeedVerifyUser = true;

            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(MofiyThumbRequest reqObj)
        {
            if (string.IsNullOrWhiteSpace(reqObj.Thumb))
            {
                return new HandleResult {
                    State = HandleStates.InvalidData,
                    Msg = "发生错误，请重试"
                };
            }

            var rowAffected = await _userRepo.UpdateUserThumbAsync(reqObj.UserId, reqObj.Thumb);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UpdateUserThumbEvent {
                    UID = reqObj.UserId,
                    Thumb = reqObj.Thumb
                });

                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = reqObj.Thumb
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
