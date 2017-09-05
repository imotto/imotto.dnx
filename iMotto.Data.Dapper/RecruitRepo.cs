using iMotto.Data.Entities;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    class RecruitRepo:RepoBase, IRecruitRepo
    {
        public async Task<int> AddRecruitApplyAsync(Entities.RecruitA ra)
        {
            throw new NotImplementedException();

            //return await ExecuteNonQueryAsync(ConnStr,
            //    "insert into T_RecruitA (UID,Title,AddTime,StartTime,Description) values (@uid,@title,@addTime,@startTime,@description)",
            //    new SqlParameter("@uid", ra.UID),
            //    new SqlParameter("@title",ra.Title),
            //    new SqlParameter("@addTime", ra.AddTime),
            //    new SqlParameter("@startTime", ra.StartTime),
            //    new SqlParameter("@description", ra.Description));
        }

        public async Task<int> RemoveRecruitApplyAsync(Entities.RecruitA ra)
        {
            throw new NotImplementedException();
            //return await ExecuteNonQueryAsync(ConnStr,
            //"delete from T_RecruitA where ID=@id",
            //new SqlParameter("@id", ra.ID));
        }

        public async Task<int> AddRecruitAsync(Entities.Recruit r)
        {
            throw new NotImplementedException();
            //return await ExecuteNonQueryAsync(ConnStr,
            //    "insert into T_Recruit (UID,Title,StartTime,StopTime,Description) values(@uid,@title,@start,@stop,@description)",
            //    new SqlParameter("@uid", r.UID),
            //    new SqlParameter("@title", r.Title),
            //    new SqlParameter("@start", r.Start),
            //    new SqlParameter("@stop", r.StopTime),
            //    new SqlParameter("@description", r.Description));
        }

        public async Task<int> AddRecruitWinnerAsync(Entities.RecruitWinner rw)
        {
            throw new NotImplementedException();
            //return await ExecuteNonQueryAsync(ConnStr,
            //    "insert into T_RecruitWinner(RID,Ranking,MID,UID,Content,Score,Up,Down,AddTime,Remark) values (@rid,@ranking,@mid,@uid,@content,@score,@up,@down,@addTime,@remark)",
            //    new SqlParameter("@rid", rw.RID),
            //    new SqlParameter("@ranking", rw.Ranking),
            //    new SqlParameter("@mid", rw.MID),
            //    new SqlParameter("@uid", rw.UID),
            //    new SqlParameter("@content", rw.Content),
            //    new SqlParameter("@score", rw.Score),
            //    new SqlParameter("@up", rw.Up),
            //    new SqlParameter("@down", rw.Down),
            //    new SqlParameter("@addTime", rw.AddTime),
            //    new SqlParameter("@remark", rw.Remark));
        }

        public async Task<List<Recruit>> GetRecruitsAsync(int start, int end)
        {
            throw new NotImplementedException();
            //var result = new List<Recruit>();
            //var sql = "select ID,UID,Title,Drafts,Involved,Actions,Charges,StartTime,StopTime,Description from T_Recruit where ID>@start and ID<=@end";

            //using (var reader = await ExecuteReaderAsync(ConnStr, sql,
            //    new SqlParameter("@start", start),
            //    new SqlParameter("@end", end)))
            //{
            //    while (reader.Read())
            //    {
            //        result.Add(GetRecruitFromReader(reader));
            //    }
            //}

            //return result;

        }

        public async Task<List<Recruit>> GetRecruitsByUserAsync(string userId, int pIndex, int pSize)
        {
            throw new NotImplementedException();
            //var result = new List<Recruit>();
            //var sql = "select ID,UID,Title,Drafts,Involved,Actions,Charges,StartTime,StopTime,Description from T_Recruit where UID=@uid order by id desc limit @start,@take";

            //using (var reader = await ExecuteReaderAsync(ConnStr, sql,
            //    new SqlParameter("@uid",userId),
            //    new SqlParameter("@start", (pIndex-1)*pSize),
            //    new SqlParameter("@take", pSize)))
            //{
            //    while (reader.Read())
            //    {
            //        result.Add(GetRecruitFromReader(reader));
            //    }
            //}

            //return result;
        }

        private Recruit GetRecruitFromReader(IDataReader reader)
        {
            var recruit = new Recruit();
            recruit.ID = reader.GetInt32(0);
            recruit.UID = reader.GetString(1);
            recruit.Title = reader.GetString(2);
            recruit.Drafts = reader.GetInt32(3);
            recruit.Involved = reader.GetInt32(4);
            recruit.Actions = reader.GetInt32(5);
            recruit.Charges = reader.GetInt32(6);
            recruit.Times = 1;
            recruit.Start = reader.GetDateTime(7);
            recruit.StopTime = reader.GetDateTime(8);
            recruit.Description = reader.GetString(9);

            return recruit;
        }
    }
}
