using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadUserHandler:BaseHandler<ReadUserRequest>
    {
        private DateTime lastLoadAt = DateTime.Today;
        private Dictionary<int, List<UserRank>> cachedRanks = new Dictionary<int, List<UserRank>>();
        private static object lockHelper = new object();
        private readonly IUserRepo _userRepo;

        public ReadUserHandler(IUserRepo userRepo) 
        {
            _userRepo = userRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadUserRequest reqObj)
        {
            //refresh cache
            if (lastLoadAt != DateTime.Today)
            {
                lock (lockHelper)
                {
                    if (lastLoadAt != DateTime.Today)
                    {
                        cachedRanks = new Dictionary<int, List<UserRank>>();
                        lastLoadAt = DateTime.Today;
                    }
                }
            }

            //read data
            List<UserRank> ranks;

            if (reqObj.TheMonth == 0)
            {
                if (!cachedRanks.TryGetValue(reqObj.TheMonth, out ranks))
                {
                    ranks = await _userRepo.ReadRankedUsersAsync(reqObj.TheMonth);

                    for (int i = 0; i < ranks.Count; i++)
                    {
                        ranks[i].Rank = i + 1;
                    }

                    cachedRanks[reqObj.TheMonth] = ranks;
                }
            }
            else
            {
                //check param
                if (IsGoodMonth(reqObj.TheMonth))
                {
                    if (!cachedRanks.TryGetValue(reqObj.TheMonth, out ranks))
                    {
                        ranks = await _userRepo.ReadRankedUsersAsync(reqObj.TheMonth);
                        for (int i = 0; i < ranks.Count; i++)
                        {
                            ranks[i].Rank = i + 1;
                        }

                        cachedRanks[reqObj.TheMonth] = ranks;
                    }
                }
                else
                {
                    ranks = new List<UserRank>();
                }
            }
            
                        
            return new HandleResult<List<UserRank>>
            {
                State = HandleStates.Success,
                Data = ranks
            };
        }

        private bool IsGoodMonth(int theMonth)
        {
            var maxDay = DateTime.Today;

            if (theMonth < 201701)
            {
                return false;
            }

            var reqMonth = theMonth % 100;

            if (reqMonth > 12 || reqMonth == 0)
            {
                //月份不对
                return false;
            }

            if (maxDay.Day >= 7)
            {
                maxDay = maxDay.AddMonths(-1);
            }
            else {
                maxDay = maxDay.AddMonths(-2);
            }

            int maxMonth = maxDay.Year * 100 + maxDay.Month;

            if (theMonth > maxMonth)
            {
                // 未结算的月份
                return false;
            }

            return true;
        }
    }
}
