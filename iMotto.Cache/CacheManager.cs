using iMotto.Cache.RedisImpl;
using iMotto.Events;
using System;
using System.Collections.Generic;

namespace iMotto.Cache
{
    public class CacheManager : ICacheManager
    {
        private bool _inited = false;
        private readonly IEventPublisher _eventPublisher;
        private readonly RedisHelper _redisHelper;
        private readonly Dictionary<Type, object> Caches = new Dictionary<Type, object>();
        
        public CacheManager(IEventPublisher eventPublisher, RedisHelper redisHelper)
        {
            _eventPublisher = eventPublisher;
            _redisHelper = redisHelper;
        }

        public void Initialize()
        {
            if (_inited)
            {
                return;
            }

            _inited = true;

            IOnlineUserCache onlineUserCache = new OnlineUserCache(_redisHelper);
            IEvaluatingMottoCache evaluatingMottoCache = new EvaluatingMottoCache(_redisHelper);
            IVerifyCodeCache verifyCodeCache = new VerifyCodeCache(_redisHelper);
            IDeviceSignatureCache deviceSignCache = new DeviceSignatureCache(_redisHelper);
            IUserInfoCache userInfoCache = new UserInfoCache(_redisHelper);
            ICollectionCache collectionCache = new CollectionCache(_redisHelper);
            ISyncRootCache syncRootCache = new SyncRootCache(_redisHelper);


            Caches.Add(typeof(IEvaluatingMottoCache), evaluatingMottoCache);
            Caches.Add(typeof(IVerifyCodeCache), verifyCodeCache);
            Caches.Add(typeof(IDeviceSignatureCache), deviceSignCache);
            Caches.Add(typeof(IUserInfoCache), userInfoCache);
            Caches.Add(typeof(IOnlineUserCache), onlineUserCache);
            Caches.Add(typeof(ICollectionCache), collectionCache);
            Caches.Add(typeof(ISyncRootCache), syncRootCache);

            _eventPublisher.RegisterEventHandler<DeviceRegEvent>(deviceSignCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<DisplayNoticeEvent>(deviceSignCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<SendVerifyCodeEvent>(verifyCodeCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UserLoginEvent>(onlineUserCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<LoadUserInfoEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<CreateMottoEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<CreateMottoEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<CreateVoteEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<CreateVoteEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<LoveMottoEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<LoveMottoEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UnloveMottoEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<UnloveMottoEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<CreateReviewEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<CreateReviewEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<RemoveReviewEvent>(evaluatingMottoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<RemoveReviewEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<CreateCollectionEvent>(userInfoCache.HandleEvent);
            //eventPublisher.RegisterEventHandler<CreateCollectionEvent>(collectionCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<CollectMottoEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<CollectMottoEvent>(collectionCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UnCollectMottoEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<UnCollectMottoEvent>(collectionCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<LoveCollectionEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<LoveCollectionEvent>(collectionCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UnLoveCollectionEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<UnLoveCollectionEvent>(collectionCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UpdateUserNameEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UpdateUserThumbEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<UpdateSexEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<LoveUserEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<UnLoveUserEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<BanUserEvent>(userInfoCache.HandleEvent);
            _eventPublisher.RegisterEventHandler<UnBanUserEvent>(userInfoCache.HandleEvent);

            _eventPublisher.RegisterEventHandler<SendPrivateMsgEvent>(userInfoCache.HandleEvent);
        }


        public T GetCache<T>() where T : class
        {
            var type = typeof(T);

            if (Caches.ContainsKey(type))
            {
                return Caches[type] as T;
            }

            return null;
        }
    }


}
