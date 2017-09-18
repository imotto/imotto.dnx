using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ModifySexHandler : BaseHandler<ModifySexRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ModifySexHandler(IEventPublisher eventPublisher, IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(ModifySexRequest reqObj)
        {
            var rowAffected = await _userRepo.UpdateUserSexAsync(reqObj.UserId, reqObj.Sex);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UpdateSexEvent
                {
                    UID = reqObj.UserId,
                    Sex = reqObj.Sex
                });

                return new HandleResult {
                    State = HandleStates.Success,
                    Msg =string.Empty,
                };
            }

            return new HandleResult
            {
                State = HandleStates.NoDataFound,
                Msg = "未找到数据"
            };
        }
    }
}
