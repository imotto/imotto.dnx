using iMotto.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace iMotto.Adapter
{
    class SignatureStore
    {
        private static object lockHelper = new object();
        private static DateTime dt = DateTime.Now;
        private static Dictionary<string, bool> RoleStates = new Dictionary<string, bool>();

        public static ILogger Logger { get; internal set; }

        public static ICacheManager CacheManager { get; internal set; }

        internal static bool PrepareSignature(string sign)
        {
            return CacheManager.GetCache<IDeviceSignatureCache>().IsDeviceActive(sign);
        }

        internal static bool VerifyUserToken(string token, string userId, string signature)
        {
            var tuple = CacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(signature);
            
            if (tuple != null && tuple.Item1.Equals(userId) && tuple.Item2.Equals(token))
            {
                return true;
            }

            Logger.LogWarning("VerifyUserToken [{0}] Falied[{1}]@[{1}]", token, userId, signature);

            return false;
        }

        internal static bool AssertUserHasRole(string uid, string role)
        {
            if (dt.AddHours(1) < DateTime.Now)
            {
                lock (lockHelper)
                {
                    RoleStates.Clear();
                }
            }

            string key = uid + role;

            lock (lockHelper)
            {
                if (RoleStates.ContainsKey(key))
                {
                    return RoleStates[key];
                }

                var hasRole = CacheManager.GetCache<IOnlineUserCache>().HasUserInRole(uid, role);
                RoleStates[key] = hasRole;

                return hasRole;
            }
        }
    }
}
