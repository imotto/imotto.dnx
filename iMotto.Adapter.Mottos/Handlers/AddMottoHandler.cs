using iMotto.Adapter.Mottos.Requests;
using System;
using System.Threading.Tasks;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Events;
using iMotto.Common;

namespace iMotto.Adapter.Mottos.Handlers
{
    class AddMottoHandler : BaseHandler<AddMottoRequest>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public AddMottoHandler(IEventPublisher eventPublisher, IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddMottoRequest reqObj)
        {
            if (reqObj.Content.Length > 500)
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "作品内容不能超过500个字符"
                };
            }

            if (SensitiveWordDetective.getInstance().isContainsSensitiveWord(reqObj.Content))
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "作品中包含敏感词语，请修正"
                };
            }

            var motto = new Motto
            {
                AddTime = DateTime.Now,
                Content = reqObj.Content,
                RecruitID = 0,
                RecruitTitle = "",
                UID = reqObj.UserId
            };

            var rowAffected = await _mottoRepo.AddMottoAsync(motto);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish<CreateMottoEvent>(new CreateMottoEvent
                {
                    Motto = motto
                });

                return new AddMottoResult
                {
                    State = HandleStates.Success,
                    Msg = "添加作品成功"
                };
            }

            return new AddMottoResult
            {
                State = HandleStates.UnkownError,
                Msg = "发生错误，请稍后重试。"
            };
        }
    }
}
