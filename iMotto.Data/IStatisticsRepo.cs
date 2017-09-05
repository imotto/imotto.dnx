using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IStatisticsRepo:IRepository
    {
        List<StatisticsViaDay> GetDayStatistics();

        Task<List<StatisticsViaUser>> GetUserStatistics(List<string> userIds);
    }
}
