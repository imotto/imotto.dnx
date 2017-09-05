using iMotto.Cache.RedisImpl;
using iMotto.Events;
using System;
using System.Collections.Generic;

namespace iMotto.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly Dictionary<Type, object> Caches = new Dictionary<Type, object>();
        
        public CacheManager(IEventPublisher eventPublisher)
        {
            IOnlineUserCache onlineUserCache = new OnlineUserCache();
            IEvaluatingMottoCache evaluatingMottoCache = new EvaluatingMottoCache();
            IVerifyCodeCache verifyCodeCache = new VerifyCodeCache();
            IDeviceSignatureCache deviceSignCache = new DeviceSignatureCache();
            IUserInfoCache userInfoCache = new UserInfoCache();
            ICollectionCache collectionCache = new CollectionCache();
            ISyncRootCache syncRootCache = new SyncRootCache();


            Caches.Add(typeof(IEvaluatingMottoCache), evaluatingMottoCache);
            Caches.Add(typeof(IVerifyCodeCache), verifyCodeCache);
            Caches.Add(typeof(IDeviceSignatureCache), deviceSignCache);
            Caches.Add(typeof(IUserInfoCache), userInfoCache);
            Caches.Add(typeof(IOnlineUserCache), onlineUserCache);
            Caches.Add(typeof(ICollectionCache), collectionCache);
            Caches.Add(typeof(ISyncRootCache), syncRootCache);
            
            eventPublisher.RegisterEventHandler<DeviceRegEvent>(deviceSignCache.HandleEvent);
            eventPublisher.RegisterEventHandler<DisplayNoticeEvent>(deviceSignCache.HandleEvent);

            eventPublisher.RegisterEventHandler<SendVerifyCodeEvent>(verifyCodeCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UserLoginEvent>(onlineUserCache.HandleEvent);

            eventPublisher.RegisterEventHandler<LoadUserInfoEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<CreateMottoEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<CreateMottoEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<CreateVoteEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<CreateVoteEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<LoveMottoEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<LoveMottoEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UnloveMottoEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<UnloveMottoEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<CreateReviewEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<CreateReviewEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<RemoveReviewEvent>(evaluatingMottoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<RemoveReviewEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<CreateCollectionEvent>(userInfoCache.HandleEvent);
            //eventPublisher.RegisterEventHandler<CreateCollectionEvent>(collectionCache.HandleEvent);

            eventPublisher.RegisterEventHandler<CollectMottoEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<CollectMottoEvent>(collectionCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UnCollectMottoEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<UnCollectMottoEvent>(collectionCache.HandleEvent);

            eventPublisher.RegisterEventHandler<LoveCollectionEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<LoveCollectionEvent>(collectionCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UnLoveCollectionEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<UnLoveCollectionEvent>(collectionCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UpdateUserNameEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UpdateUserThumbEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<UpdateSexEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<LoveUserEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<UnLoveUserEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<BanUserEvent>(userInfoCache.HandleEvent);
            eventPublisher.RegisterEventHandler<UnBanUserEvent>(userInfoCache.HandleEvent);

            eventPublisher.RegisterEventHandler<SendPrivateMsgEvent>(userInfoCache.HandleEvent);
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
