using iMotto.Adapter.Users.Requests;
using iMotto.Common;
using iMotto.Data;
using iMotto.Events;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class SendMsgHandler : BaseHandler<SendMsgRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMsgRepo _msgRepo;

        public SendMsgHandler(IEventPublisher eventPublisher, IMsgRepo msgRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _msgRepo = msgRepo;
        }

        protected override async Task<HandleResult> HandleCoreAsync(SendMsgRequest reqObj)
        {

            if (string.IsNullOrEmpty(reqObj.Content))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "消息内容不能为空"
                };
            }

            if (reqObj.UserId.Equals(reqObj.TUID))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "不能发私信给自已"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Content))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "信息中包含敏感词语，请修正"
                };
            }

            int rowAffected = await _msgRepo.SendMsgAsync(reqObj.UserId, reqObj.TUID, reqObj.Content, DateTime.Now);

            _eventPublisher.Publish(new SendPrivateMsgEvent {
                SUID = reqObj.UserId,
                TUID = reqObj.TUID,
                Msg = reqObj.Content
            });

            return new HandleResult
            {
                State = HandleStates.Success,
                Msg = string.Empty
            };
        }
    }
}
