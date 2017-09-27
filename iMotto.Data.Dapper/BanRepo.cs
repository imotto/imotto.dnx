using iMotto.Data.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace iMotto.Data.Dapper
{
    public class BanRepo:IBanRepo
    {
        private IConnectionProvider _connProvider;
        public BanRepo(IConnectionProvider connectionProvider)
        {
            _connProvider = connectionProvider;
        }

        public async Task<int> AddBanAsync(Ban ban, bool removeFollow, bool resetMutual)
        {
            var rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {                
                var tran = conn.BeginTransaction();

                try
                {
                    await conn.ExecuteAsync("insert into Bans(SUID,TUID,BanTime) values (@SUID,@TUID,@BanTime)", ban, tran);

                    if (removeFollow)
                    {
                        await conn.ExecuteAsync("delete from Follows where SUID=@SUID and TUID=@TUID",
                            new
                            {
                                SUID = ban.SUID,
                                TUID = ban.TUID
                            }, tran);                      
                    }

                    if (resetMutual)
                    {
                        await conn.ExecuteAsync("update Follows set IsMutual=@IsMutual where SUID=@suid and TUID= @tuid",
                            new
                            {
                                IsMutual = 0,
                                SUID = ban.TUID,
                                TUID = ban.SUID
                            }, tran);
                    }

                    await conn.ExecuteAsync("update UserStatistics set Bans=Bans+1, Follows=Follows-@ReduceFollow where UID=@UID", new
                    {
                        ReduceFollow = removeFollow ? 1 : 0,
                        UID = ban.SUID
                    }, tran);
                 
                    if (removeFollow)
                    {
                        await conn.ExecuteAsync("update UserStatistics set Followers=Followers-1 where UID=@uid", new
                        {
                            UID = ban.TUID
                        }, tran);
                    }

                    tran.Commit();
                }
                catch {
                    try
                    {
                        tran.Rollback();
                    }
                    catch { }
                    throw;
                }
            }
            
            return rowAffected;
        }

        public async Task<int> RemoveBanAsync(Ban ban)
        {
            var rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    await conn.ExecuteAsync("delete from Bans where SUID=@SUID and TUID=@TUID", ban, tran);
                    await conn.ExecuteAsync("update UserStatistics set Bans=Bans-1 where UID=@uid", new { UID = ban.SUID }, tran);
                    tran.Commit();
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch { }
                    throw;
                }
            }

            return rowAffected;
        }
    }
}
