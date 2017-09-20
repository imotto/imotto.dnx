using iMotto.Adapter.Readers.Requests;
using iMotto.Data;
using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadStatisticsHandler:BaseHandler<ReadStatisticsRequest>
    {
        private static Dictionary<int, List<StatisticsViaDay>> statistics;
        private static object lockHelper = new object();
        private const int MIN_MON = 201701;
        private readonly IStatisticsRepo _statisticsRepo;

        public ReadStatisticsHandler(IStatisticsRepo statisticsRepo)
        {
            _statisticsRepo = statisticsRepo;
        }

        protected override Task<HandleResult> HandleCoreAsync(ReadStatisticsRequest reqObj)
        {
            if (statistics == null)
            {
                lock(lockHelper)
                {
                    if (statistics == null)
                    {
                        var stData = _statisticsRepo.GetDayStatistics();
                    }
                }
                
            }

            if (statistics.ContainsKey(reqObj.TheMonth))
            {
                return Task.FromResult<HandleResult>(new HandleResult<List<StatisticsViaDay>>
                {
                    State = HandleStates.Success,
                    Data = statistics[reqObj.TheMonth]
                });
            }

            return Task.FromResult(new HandleResult
            {
                State = HandleStates.NoDataFound,
                Msg="未找到相关数据"
            });
        }

        private void CacheStatistics(List<StatisticsViaDay> stData)
        {
            statistics = new Dictionary<int, List<StatisticsViaDay>>();
            var monthes = stData.Select(s => s.Day / 100).Distinct();

            int monthBegin;

            foreach (var month in monthes)
            {
                monthBegin = month * 100;

                var sts = stData.Where(s => s.Day > monthBegin && s.Day < monthBegin + 32).OrderBy(s => s.Day).ToList();
                statistics.Add(month, sts);
            }
        }
    }
}
