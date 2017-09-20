using iMotto.Adapter.Readers.Results;
using iMotto.Data;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserBadgeHandler : BaseHandler<AuthedRequest>
    {
        private readonly IUserRepo _userRepo;

        public ReadUserBadgeHandler(IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _userRepo = userRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(AuthedRequest reqObj)
        {
            int count = await _userRepo.GetUserUnReadMsgsAsync(reqObj.UserId);

            return new ReadUserBadgeResult
            {
                State = HandleStates.Success,
                Msg = string.Empty,
                Badge = count
            };
        }
    }
}
