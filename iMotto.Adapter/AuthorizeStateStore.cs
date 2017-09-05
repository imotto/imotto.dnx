//using iMotto.Cache;
//using System;
//using System.Collections.Generic;

//namespace iMotto.Adapter
//{
//    static class RoleStateStore
//    {
//        private static object lockHelper = new object();
//        private static DateTime dt = DateTime.Now;
//        private static Dictionary<string, bool> RoleStates = new Dictionary<string, bool>();
//        internal static bool AssertUserHasRole(string uid, string role)
//        {
//            if (dt.AddHours(1) < DateTime.Now)
//            {
//                lock (lockHelper)
//                {
//                    RoleStates.Clear();
//                }
//            }

//            string key = uid + role;

//            lock (lockHelper)
//            {
//                if (RoleStates.ContainsKey(key))
//                {
//                    return RoleStates[key];
//                }
                
//                var hasRole = CacheManager.Instance.GetCache<IOnlineUserCache>().HasUserInRole(uid, role);
//                RoleStates[key] = hasRole;

//                return hasRole;
//            }


//        }
//    }

//}