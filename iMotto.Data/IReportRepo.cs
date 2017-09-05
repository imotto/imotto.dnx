using iMotto.Data.Entities;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IReportRepo:IRepository
    {
        Task<int> AddReportAsync(Report r);

        Task<int> UpdateReportAsync(Report r);


    }
}
