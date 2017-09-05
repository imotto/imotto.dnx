using Dapper;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class ReportRepo:IReportRepo
    {
        private readonly IConnectionProvider _connProvider;

        public ReportRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }
        
        public async Task<int> AddReportAsync(Entities.Report r)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync(@"insert into Reports (UID,Type,TargetID,Reason,Status,ReportTime) 
                    values(@UID,@Type,@TargetID,@Reason,@Status,@ReportTime)", r);
            }
        }

        public async Task<int> UpdateReportAsync(Entities.Report r)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync(@"update T_Report set Status=@Status,ProcessUID=@ProcessUID,Result=@Result,ProcessTime=@ProcessTime
                    where ID=@ID", r);
            }
        }
    }
}
