using System;
using System.Threading.Tasks;
using iMotto.Adapter.Mottos.Requests;
using iMotto.Data;
using iMotto.Adapter.Mottos.Results;
using iMotto.Data.Entities;
using iMotto.Common;
using iMotto.Cache;
using iMotto.Events;

namespace iMotto.Adapter.Mottos.Handlers
{
    class AddVoteHandler:BaseHandler<AddVoteRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMottoRepo _mottoRepo;

        public AddVoteHandler(ICacheManager cacheManager,
            IEventPublisher eventPublisher, 
            IMottoRepo mottoRepo)
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _mottoRepo = mottoRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddVoteRequest reqObj)
        {
            var voteTime = DateTime.Now;
            if (reqObj.TheDay < 20161001 || reqObj.TheDay > Utils.GetTheDay(voteTime.Date)
                || reqObj.Support < -1 || reqObj.Support > 1)
            {
                return new HandleResult
                {
                    State = HandleStates.InvalidData,
                    Msg = "请求数据有误"
                };
            }

            var motto = _cacheManager.GetCache<IEvaluatingMottoCache>().FindMotto(reqObj.TheDay, reqObj.MID);


            if (motto == null || motto.AddTime.AddDays(7) <= voteTime)
            {
                return new AddVoteResult
                {
                    State = HandleStates.Success,
                    Msg = "评估已结束"
                };
            }

            var uInfoCache = _cacheManager.GetCache<IUserInfoCache>();
            if (uInfoCache.HasVoted(reqObj.UserId, reqObj.MID) != 9) 
            {
                return new AddVoteResult
                {
                    State = HandleStates.Success,
                    Msg = "已投过票"
                };
            }

            if(reqObj.Support == 0) //中立
            {
                _eventPublisher.Publish(new CreateVoteEvent
                {
                    UID = reqObj.UserId,
                    MID = reqObj.MID,
                    TheDay = reqObj.TheDay,
                    Vote = reqObj.Support
                });

                return new AddVoteResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };

            }

            var vote = new Vote
            {
                MID = reqObj.MID,
                Oppose = reqObj.Support == -1 ? 1 : 0,
                Support = reqObj.Support == 1 ? 1 : 0,
                UID = reqObj.UserId,
                TheDay = reqObj.TheDay,
                VoteTime = voteTime
            };

            var rowAffected = await _mottoRepo.AddVoteAsync(vote);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new CreateVoteEvent {

                    UID = reqObj.UserId,
                    MID = reqObj.MID,
                    TheDay = reqObj.TheDay,
                    Vote = reqObj.Support
                });

                return new AddVoteResult
                {
                    State=HandleStates.Success,
                    Msg = string.Empty

                };
            }

            return new AddVoteResult
            {
                State = HandleStates.NoDataFound,
                Msg="投票的偶得已删除"
            };


        }
    }
}
