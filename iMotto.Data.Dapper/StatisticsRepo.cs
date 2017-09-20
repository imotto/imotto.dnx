using Dapper;
using iMotto.Data.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class StatisticsRepo:RepoBase,IStatisticsRepo
    {
        private IConnectionProvider _connProvider;
        public StatisticsRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }

        public List<StatisticsViaDay> GetDayStatistics()
        {
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = conn.Query<StatisticsViaDay>("select ID as Day,Mottos,TotalScore from T_DayStatistics");

                return tmp.AsList();
            }
        }

        public Task<List<StatisticsViaUser>> GetUserStatistics(List<string> userIds)
        {
            throw new NotImplementedException();
            //var result = new List<StatisticsViaUser>();
            //var sql = "select UID,Mottos,Recruits,Collections,Reviews,Votes,LovedMottos,LovedTreasures,Revenue,Balance,Follows,Followers,Bans from UserStatistics where UID in ({0})";
            //var parameters = Helper.BuildDynamicParameters<string>(ref sql, userIds);
                       

            //using (var reader = await ExecuteReaderAsync(ConnStr, sql, parameters.ToArray()))
            //{
            //    while (reader.Read())
            //    {
            //        var s = new StatisticsViaUser();
            //        s.UID = reader.GetString(0);
            //        s.Mottos = reader.GetInt32(1);
            //        s.Recruits = reader.GetInt32(2);
            //        s.Collections = reader.GetInt32(3);
            //        s.Reviews = reader.GetInt32(4);
            //        s.Votes = reader.GetInt32(5);
            //        s.LovedMottos = reader.GetInt32(6);
            //        s.LovedCollections = reader.GetInt32(7);
            //        s.Revenue = reader.GetInt32(8);
            //        s.Balance = reader.GetInt32(9);
            //        s.Follows = reader.GetInt32(10);
            //        s.Followers = reader.GetInt32(11);
            //        s.Bans = reader.GetInt32(12);

            //        result.Add(s);
            //    }

            //    reader.Close();
            //}

            //return result;
        }
    }
}
