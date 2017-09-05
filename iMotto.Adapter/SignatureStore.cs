//using iMotto.Cache;
//using log4net;
//using System.Collections.Generic;

//namespace iMotto.Adapter
//{
//    static class SignatureStore
//    {
//        private static ILog logger = LogManager.GetLogger("SignatureStore");

//        internal static bool PrepareSignature(string sign)
//        {
//            return CacheManager.Instance.GetCache<IDeviceSignatureCache>().IsDeviceActive(sign);
//        }

//        internal static bool VerifyUserToken(string token, string userId, string signature)
//        {
//            var tuple = CacheManager.Instance.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(signature);
            

//            if (tuple!=null && tuple.Item1.Equals(userId) && tuple.Item2.Equals(token))
//            {
//                return true;
//            }

//            if (logger.IsInfoEnabled)
//            {
//                logger.InfoFormat("VerifyUserToken [{2}] Falied[{0}]@[{1}]", userId, signature, token);
//            }

//            return false;
//        }
//    }
//}
