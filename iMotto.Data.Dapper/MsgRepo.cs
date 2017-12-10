using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using iMotto.Data.Entities;
using Dapper;

namespace iMotto.Data.Dapper
{
    public class MsgRepo : IMsgRepo
    {

        private IConnectionProvider _connProvider;

        public MsgRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }


        public async Task<List<Notice>> GetNoticesAsync(string userId, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {

                var tmp = await conn.QueryAsync<Notice>(@"select ID,Title,Content,Type,CreateTime as SendTime,State,TargetID,TargetInfo
                    from Notices where UID=@UID Order by ID desc limit @Skip, @Take",
                    new
                    {
                        UID = userId,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }            
        }

        public async Task<int> SetNoticeReadAsync(long id, string userId)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Notices set State=1 where ID=@NID and UID=@UID",
                    new {
                        NID = id,
                        UID = userId
                    });                
            }
        }

        public async Task<List<RecentTalk>> GetRecentTalksAsync(string uid, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<RecentTalk>(@"select WithUID,Content,Direction,LastTime,Msgs 
                    from RecentTalks where UID=@UID order by LastTime desc limit @Skip, @Take",
                    new
                    {
                        UID = uid,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        public async Task<List<Talk>> GetTalkMsgsAsync(string uid, string withUid, long start, int take)
        {
            bool needReverse = false;
            bool clearUnRead = false;
            
            string sql;
            if (start == 0)
            {
                needReverse = true;
                clearUnRead = true;
                sql = $"select ID,Content,Direction,SendTime from Talks where uid=@UID and WithUID=@WithUID order by id desc limit 0,{Math.Abs(take)}";
            }
            else if (take < 0)
            {
                needReverse = true;
                sql = $"select ID,Content,Direction,SendTime from Talks where ID<{start} and uid=@UID and WithUID=@WithUID order by id desc limit 0,{Math.Abs(take)}";
            }
            else if (take > 0)
            {
                sql = $"select ID,Content,Direction,SendTime from Talks where ID>{start} and uid=@UID and WithUID=@WithUID order by id asc limit 0,{take}";
                clearUnRead = true;
            }
            else
            {
                return new List<Talk>();
            }
            

            using (var conn = _connProvider.GetConnection())
            {
                var param = new
                {
                    UID = uid,
                    WithUID = withUid
                };

                var tmp = await conn.QueryAsync<Talk>(sql, param);
                


                if (clearUnRead)
                {
                    await conn.ExecuteAsync(@"update RecentTalks set Msgs = 0 where UID=@UID and WithUID=@WithUID", param);
                }

                var result = tmp.AsList();
                if (needReverse)
                {
                    result.Reverse();
                }

                return result;
            }
            
        }

        public async Task<int> SendMsgAsync(string uid, string tuid, string content, DateTime sendTime)
        {
            /*
             
             `P_SEND_MSG`(<{in IN_UID varchar(32)}>, <{in IN_WITH_UID varchar(32)}>, <{in IN_CONTENT varchar(500)}>, <{in IN_SEND_TIME datetime}>);


             */

            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("P_SEND_MSG",
                    new
                    {
                        IN_UID = uid,
                        IN_WITH_UID = tuid,
                        IN_CONTENT = content,
                        IN_SEND_TIME = sendTime
                    },
                    commandType: CommandType.StoredProcedure);                
            }
        }
    }
}
