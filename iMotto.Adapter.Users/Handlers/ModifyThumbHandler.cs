using iMotto.Adapter.Users.Requests;
using iMotto.Data;
using iMotto.Events;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class ModifyThumbHandler : BaseHandler<MofiyThumbRequest>
    {
        private readonly string thumbImgUrlFmt;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;

        public ModifyThumbHandler(IEventPublisher eventPublisher, IUserRepo userRepo) 
        {
            var tmp = "";//ConfigurationManager.AppSettings.Get("UserPhotoImgUrlFmt");
            if (string.IsNullOrEmpty(tmp))
            {
                thumbImgUrlFmt = "http://139.224.227.37/pic/{0}";
            }
            else {
                thumbImgUrlFmt = tmp;
            }

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

            //todo: 移动图片到对象存储服务器

            var thumb = string.Format(thumbImgUrlFmt, reqObj.Thumb);



            var rowAffected = await _userRepo.UpdateUserThumbAsync(reqObj.UserId, thumb);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new UpdateUserThumbEvent {
                    UID = reqObj.UserId,
                    Thumb = thumb
                });

                return new HandleResult
                {
                    State = HandleStates.Success,
                    Msg = thumb
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
