using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserMottosHandler:BaseHandler<ReadUserMottosRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepo _userRepo;
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadUserMottosHandler(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IUserRepo userRepo,
            IMottoRepo mottoRepo, 
            ModelBuilder modelBuilder) 
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _userRepo = userRepo;
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserMottosRequest reqObj)
        {
            List<Motto> mottos = null;
            bool isOthers = true;
            var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
            mottos = await _mottoRepo.GetMottosByUserAsync(reqObj.UID, reqObj.PIndex, reqObj.PSize);

            if (uInfo != null && uInfo.Item1 == reqObj.UID)
            {
                isOthers = false;
            }
            
            var data = BuildUserMottoModels(mottos, reqObj.UID);
            
            if (isOthers)
            {
                await _modelBuilder.DecorateUserRelatedData(data, uInfo.Item1);
            }

            return new HandleResult<List<MottoModel>>
            {
                State = HandleStates.Success,
                Data = data
            };
        }

        public List<MottoModel> BuildUserMottoModels(List<Motto> mottos, string uid)
        {
            var user = _cacheManager.GetCache<IUserInfoCache>().GetUserById(uid);
            if (user == null)
            {
                user = _userRepo.GetUserById(uid);
                if (user != null)
                {
                    _eventPublisher.Publish<LoadUserInfoEvent>(new LoadUserInfoEvent
                    {
                        UserInfo = user
                    });
                }
                else
                {
                    user = new User
                    {
                        UserName = "佚名",
                        Thumb = string.Empty
                    };
                }
            }

            var result = new List<MottoModel>();

            foreach (var item in mottos)
            {
                var model = new MottoModel
                {
                    ID = item.ID,
                    AddTime = item.AddTime,
                    Content = item.Content,
                    Down = item.Down,
                    Loves = item.Loves,
                    RecruitID = item.RecruitID,
                    RecruitTitle = item.RecruitTitle,
                    Reviews = item.Reviews,
                    Score = item.Score,
                    State = item.AddTime.AddDays(7) < DateTime.Now ? 1 : 0,
                    Up = item.Up,
                    Vote = 0, //投票状态置为（0）中立，读取自己的作品时，直接作为中立未投票，读取其它的人的作品时，返回的记录都是评估结束的。
                    UID = item.UID,
                    UserName = user.DisplayName,
                    UserThumb = user.Thumb
                };

                result.Add(model);
            }

            return result;
        }
    }
}
