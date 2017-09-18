using iMotto.Adapter.Users.Requests;
using iMotto.Adapter.Users.Results;
using iMotto.Cache;
using iMotto.Data;
using iMotto.Data.Entities;
using iMotto.Data.Entities.Models;
using iMotto.Events;
using System;
using System.Threading.Tasks;

namespace iMotto.Adapter.Users.Handlers
{
    class AddFollowHandler:BaseHandler<AddFollowRequest>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IFollowRepo _followRepo;
        private readonly IUserRepo _userRepo;

        public AddFollowHandler(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IFollowRepo followRepo,
            IUserRepo userRepo) 
        {
            NeedVerifyUser = true;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _followRepo = followRepo;
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(AddFollowRequest reqObj)
        { 
            UserRelation relation = await _cacheManager.GetCache<IUserInfoCache>().GetRelation(reqObj.UserId, reqObj.TargetUId);

            if (relation.SLoveT)
            {
                return new AddFollowResult
                {
                    State = HandleStates.Success,
                    Msg = string.Empty
                };
            }
            
            var follow = new Follow
            {
                FollowTime = DateTime.Now,
                IsMutual = relation.TLoveS,
                SUID = reqObj.UserId,
                SUname = TryGetUserName(reqObj.UserId),
                TUID = reqObj.TargetUId
            };

            var rowAffected = await _followRepo.AddFollowAsync(follow, relation.SBanT);

            if (rowAffected > 0)
            {
                _eventPublisher.Publish(new LoveUserEvent {
                    SUID = reqObj.UserId,
                    TUID = reqObj.TargetUId
                });

                if (relation.SBanT)
                {
                    _eventPublisher.Publish(new UnBanUserEvent {
                        SUID = reqObj.UserId,
                        TUID = reqObj.TargetUId
                    });
                }

                return new AddFollowResult {
                    State=HandleStates.Success,
                    Msg =string.Empty
                };
            }

            return new AddFollowResult {
                State=HandleStates.NoDataFound,
                Msg="未找到数据"
            };
        }

        private string TryGetUserName(string uid) {
            string uname = "someone";
            var user = _cacheManager.GetCache<IUserInfoCache>().GetUserById(uid);
            if (user == null)
            {
                user = _userRepo.GetUserById(uid);
                if (user != null)
                {
                    uname = user.DisplayName;

                    _eventPublisher.Publish<LoadUserInfoEvent>(new LoadUserInfoEvent
                    {
                        UserInfo = user
                    });
                }
            }
            else {
                uname = user.DisplayName;
            }

            return uname;
        }
    }
}
