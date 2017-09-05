using System;
using iMotto.Events;
using iMotto.Data.Entities;
using iMotto.Data.Entities.Models;

namespace iMotto.Cache.RedisImpl
{
    class DeviceSignatureCache : IDeviceSignatureCache
    {
        private const string KEY_DEV_FMT = "DEV{0}";
        private const string KEY_AWARD_NOTICE = "AWDN{0}";
        private const string KEY_AWARDEE_NOTICE = "ADEN{0}";

        public void HandleEvent(DeviceRegEvent @event)
        {
            RedisHelper.StringSet(string.Format(KEY_DEV_FMT, @event.Signature), DateTime.Now.ToString("yyyyMMddHHmmss"),
                TimeSpan.FromDays(31));
        }

        public bool IsDeviceActive(string sign)
        {
            var result = RedisHelper.StringGet(string.Format(KEY_DEV_FMT, sign));
            return !string.IsNullOrWhiteSpace(result);
        }

        public DisplayNotice ShouldDisplayNotice(string sign, int theMonth)
        {
            var redis = RedisHelper.GetDatabase();
            string awardeeKey = string.Format(KEY_AWARDEE_NOTICE, theMonth);
            if (redis.KeyExists(awardeeKey))
            {
                if (!RedisHelper.SetContains(awardeeKey, sign))
                {
                    return DisplayNotice.Awardee;
                }
            }

            string awardKey = string.Format(KEY_AWARD_NOTICE, theMonth);

            if (redis.KeyExists(awardKey))
            {
                if (!RedisHelper.SetContains(awardKey, sign))
                {
                    return DisplayNotice.Award;
                }
            }

            return DisplayNotice.None;
        }

        public void HandleEvent(DisplayNoticeEvent @event)
        {
            var redis = RedisHelper.GetDatabase();
            switch (@event.DisplayNotice)
            {
                case DisplayNotice.Award:
                    string awardKey = string.Format(KEY_AWARD_NOTICE, @event.TheMonth);

                    if (redis.KeyExists(awardKey))
                    {
                        redis.SetAdd(awardKey, @event.Sign);
                    }

                    break;
                case DisplayNotice.Awardee:
                    string awardeeKey = string.Format(KEY_AWARDEE_NOTICE, @event.TheMonth);
                    if (redis.KeyExists(awardeeKey))
                    {
                        redis.SetAdd(awardeeKey, @event.Sign);
                    }

                    break;
            }
        }
    }
}
