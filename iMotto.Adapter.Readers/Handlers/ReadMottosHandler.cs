using iMotto.Adapter.Readers.Requests;
using iMotto.Cache;
using iMotto.Common;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadMottosHandler : BaseHandler<ReadMottosRequest>
    {
        private static ConcurrentDictionary<int, List<Motto>> activeMottos = new ConcurrentDictionary<int, List<Motto>>();
        private static readonly DateTime VERY_BEINGING = new DateTime(2017, 1, 1);
        private readonly ICacheManager _cacheManager;
        private readonly IMottoRepo _mottoRepo;
        private readonly ModelBuilder _modelBuilder;

        public ReadMottosHandler(ICacheManager cacheManager, IMottoRepo mottoRepo, ModelBuilder modelBuilder)
        {
            _cacheManager = cacheManager;
            _mottoRepo = mottoRepo;
            _modelBuilder = modelBuilder;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadMottosRequest reqObj)
        {
            var ret = await FetchData(reqObj);

            if (ret.Data != null && ret.Data.Count > 0)
            {
                var uInfo = _cacheManager.GetCache<IOnlineUserCache>().GetOnlineUserViaSignature(reqObj.Sign);
                if (uInfo != null)
                {   
                    await _modelBuilder.DecorateUserRelatedData(ret.Data, uInfo.Item1);
                }
            }

            return ret;
        }

        private async Task<HandleResult<List<MottoModel>>> FetchData(ReadMottosRequest reqObj)
        {
            var thisDay = DateTime.Today;
            var activeStart = DateTime.Today.AddDays(-7);
            DateTime theDay = thisDay;
            if (reqObj.TheDay != 0)
            {
                theDay = reqObj.TheDay.ToDateTime();
            }

            if (theDay < VERY_BEINGING || theDay > thisDay)
            {
                return new HandleResult<List<MottoModel>>
                {
                    State = HandleStates.Success,
                    Msg = "没有找到当天的Motto。",
                    Data = null
                };
            }

            if (theDay < activeStart)
            {
                var mottos = await _mottoRepo.GetMottosByDayAsync(theDay, reqObj.PIndex, reqObj.PSize);

                return new HandleResult<List<MottoModel>>
                {
                    State = HandleStates.Success,
                    Data = _modelBuilder.BuildMottoModels(mottos)
                };
            }

            var tmp = await _cacheManager.GetCache<IEvaluatingMottoCache>().GetMottos(Utils.GetTheDay(theDay), reqObj.PIndex, reqObj.PSize);

            if (tmp != null && tmp.Length > 0)
            {
                var data = tmp.ToList();
                return new HandleResult<List<MottoModel>>
                {
                    State = HandleStates.Success,
                    Data = _modelBuilder.BuildMottoModels(data)
                };
            }

            return new HandleResult<List<MottoModel>>
            {
                State = HandleStates.Success,
                Data = null,
                Msg = "没有更多数据了。"
            };
        }
    }
}
