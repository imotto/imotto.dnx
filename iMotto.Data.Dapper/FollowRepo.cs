using Dapper;
using iMotto.Data.Entities;
using System;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class FollowRepo:IFollowRepo
    {
        public IConnectionProvider _connProvider;

        public FollowRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }


        public async Task<int> AddFollowAsync(Follow follow, bool removeBan)
        {
            var rowaffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowaffected = await conn.ExecuteAsync("insert into T_Follow(SUID,TUID,IsMutual,FollowTime) values (@SUID,@TUID,@IsMutual,@FollowTime)", follow, tran);                    
                    
                    if (follow.IsMutual)
                    {
                        await conn.ExecuteAsync("update T_Follow set IsMutual=1 where SUID=@SUID and TUID= @TUID",
                            new { SUID = follow.TUID, TUID = follow.SUID }, tran);
                    }

                    if (removeBan)
                    {
                        await conn.ExecuteAsync("delete T_Ban where SUID=@SUID and TUID=@TUID",
                            new { SUID = follow.SUID, TUID = follow.TUID }, tran);
                    }
                    
                    await conn.ExecuteAsync("update T_UserStatistics set Follows=Follows+1, Bans=Bans-@Ban where UID=@UID",
                        new { Ban = removeBan ? 1 : 0, UID = follow.SUID }, tran);
                    
                    await conn.ExecuteAsync("update T_UserStatistics set Followers=Followers+1 where UID=@uid", 
                        new { UID = follow.TUID }, tran);                  

                    await conn.ExecuteAsync(@"insert into T_Notice(UID,Title,Content,Type,State,TargetID, TargetInfo,CreateTime)
	                    values(@UID,@Title, @Content, @Type, @State,@TargetId,@TargetInfo,@CreateTime)",
                        new
                        {
                            UID = follow.TUID,
                            Title = "有人喜欢你",
                            Content = string.Format("{0}将你添加为他喜欢的人", follow.SUname),
                            Type = 1,
                            State = 0,
                            TargetId = follow.SUID,
                            TargetInfo = follow.SUname,
                            CreateTime = DateTime.Now
                        }, tran);

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

            return rowaffected;
        }

        public async Task<int> RemoveFollowAsync(Follow follow)
        {
            var rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffected = await conn.ExecuteAsync("delete from T_Follow where SUID=@SUID and TUID=@TUID",
                        follow, tran);

                    if (follow.IsMutual)
                    {
                        await conn.ExecuteAsync("update T_Follow set IsMutual=0 where SUID=@SUID and TUID= @TUID",
                            new
                            {
                                SUID = follow.TUID,
                                TUID = follow.SUID
                            }, tran);                        
                    }

                    await conn.ExecuteAsync("update T_UserStatistics set Follows=Follows-1 where UID=@UID",
                        new { UID = follow.SUID }, tran);

                    await conn.ExecuteAsync("update T_UserStatistics set Followers=Followers-1 where UID=@UID",
                        new { UID = follow.TUID }, tran);                   

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
