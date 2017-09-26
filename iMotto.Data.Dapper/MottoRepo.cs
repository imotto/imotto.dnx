using Dapper;
using iMotto.Common;
using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace iMotto.Data.Dapper
{
    public class MottoRepo: IMottoRepo
    {
        private readonly IConnectionProvider _connProvider;

        public MottoRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }

        public async Task<int> AddMottoAsync(Motto m)
        {
            int rowAffected = 0;
                        
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    var id = await conn.QueryFirstOrDefaultAsync<long>("insert into Mottos(UID,TheDay, Score, Content,RecruitID,RecruitTitle,AddTime) " +
                        "values(@UID,@TheDay,@Score,@Content,@RID,@RTitle,@AddTime); select @@IDENTITY;", new {
                        UID = m.UID,
                        TheDay = Utils.GetTheDay(m.AddTime),
                        Score = 0f,
                        Content = m.Content,
                        RID = m.RecruitID,
                        RTitle = m.RecruitTitle,
                        AddTime = m.AddTime
                    }, tran);


                    if (id != 0)
                    {
                        rowAffected = 1;
                        //var theday = m.AddTime.Year * 10000 + m.AddTime.Month * 100 + m.AddTime.Day;
                        //await conn.ExecuteAsync("update T_DayStatistics set Mottos=Mottos+1 where ID=@theday",
                        //    new
                        //    {
                        //        TheDay = theday
                        //    }, tran);

                        await conn.ExecuteAsync("update UserStatistics set Mottos = Mottos+1 where UID=@uid",
                            new
                            {
                                UID = m.UID
                            },
                            tran);

                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch {
                    };

                    throw;
                }
            }

            return rowAffected;
        }

        public async Task<int> AddLoveMottoAsync(LoveMotto lm)
        {
            int rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffected = await conn.ExecuteAsync("insert into LoveMottos (UID,MID,TheDay, LoveTime) values(@UID,@MID,@TheDay, @LoveTime)",
                        lm, tran);
                    
                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update Mottos set loves=Loves+1 where id=@MID",
                            new
                            {
                                MID = lm.MID
                            },
                            tran);

                        
                        await conn.ExecuteAsync("update UserStatistics set LovedMottos=LovedMottos+1 where uid=@UID",
                            new
                            {
                                UID = lm.UID
                            },
                            tran);
                        
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch {
                    }

                    throw;
                }

            }


            return rowAffected;
        }

        public async Task<int> RemoveLoveMottoAsync(LoveMotto lm)
        {
            int rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffected = await conn.ExecuteAsync("delete from LoveMottos where UID=@UID and MID=@MID",
                        lm, tran);
                    
                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update Mottos set Loves=Loves-1 where Id=@MID",
                            new
                            {
                                MID = lm.MID
                            }, tran);

                        await conn.ExecuteAsync("update UserStatistics set LovedMottos=LovedMottos-1 where UID=@UID",
                            new {
                                UID = lm.UID
                            },
                            tran);

                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }

            }


            return rowAffected;
        }

        public async Task<int> AddReviewAsync(Review review)
        {
            var rowAffected = 0;           

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    var sql = "insert into Reviews (MID,UID,Content,AddTime) values(@MID,@UID,@Content,@AddTime)";
                    rowAffected = await conn.ExecuteAsync(sql, review, tran);                    
                    
                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update Motto set Reviews=Reviews+1 where Id=@MID",
                            new
                            {
                                MID = review.MID
                            }, tran);

                        await conn.ExecuteAsync("update UserStatistics set Reviews=Reviews+1 where UID=@UID",
                            new {
                                UID = review.UID
                            }, tran);

                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch {
                    }

                    throw;
                }
            }

            return rowAffected;
        }

        public async Task<int> RemoveReviewAsync(Review review)
        {
            var rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    rowAffected = await conn.ExecuteAsync("delete from Reviews where MID=@MID and Id=@ID and UID=@UID", review, tran);

                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update Mottos set Reviews=Reviews-1 where Id=@MID",
                            new
                            {
                                MID = review.MID

                            }, tran);

                        await conn.ExecuteAsync("update UserStatistics set Reviews=Reviews-1 where UID=@UID",
                            new
                            {
                                UID = review.UID
                            }, tran);

                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }

            return rowAffected;
        }

        public async Task<int> AddReviewVoteAsync(ReviewVote rv)
        {
            var rowAffected = 0;            

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.Transaction = tran;
                    if (rv.Support == 1)
                    {
                        rowAffected = await conn.ExecuteAsync("Insert into ReviewVotes (MID,ReviewID,UID,Support,Oppose,VoteTime) " +
                            "values(@MID,@ReviewID,@UID,@Support,@Oppose,@VoteTime)",
                            rv, tran);                        
                    }
                    else
                    {                        
                        rowAffected = await conn.ExecuteAsync("delete ReviewVotes where MID=@MID and ReviewID=@ReviewID and UID=@UID", rv, tran);
                    }

                    if (rowAffected > 0)
                    {                       
                        if (rv.Support == 1)
                        {
                            await conn.ExecuteAsync("update Reviews set Up=Up+1 where MID=@mid and ID=@reviewId",
                                new
                                {
                                    MID = rv.MID,
                                    ReviewId = rv.ReviewID
                                }, tran);
                        }
                        else
                        {
                            await conn.ExecuteAsync("update Reviews set Up=Up-1 where MID=@mid and ID=@reviewId",
                                new
                                {
                                    MID = rv.MID,
                                    ReviewId = rv.ReviewID
                                }, tran);
                        }
                        
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }

            return rowAffected;

        }

        public async Task<int> AddVoteAsync(Vote v)
        {
            int rowAffected = 0;            
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    rowAffected = await conn.ExecuteAsync("insert into Votes (MID,UID,TheDay,Support,Oppose,VoteTime) values (@MID,@UID,@TheDay,@Support,@Oppose,@VoteTime)",
                        v, tran);

                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update UserStatistics set Votes=Votes+1 where UID=@UID", new { UID = v.UID }, tran);

                        await conn.ExecuteAsync("update Mottos set Up=Up+@Up,Down=Down+@Down where ID=@MID", 
                            new {
                                Up= v.Support,
                                Down = v.Oppose,
                                MID = v.MID
                            }, tran);                        

                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
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


        public List<Motto> GetMottosBetween(long start, long end)
        {
            var result = new List<Motto>();            

            using (var conn = _connProvider.GetConnection())
            {
                var sql = "select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime from Mottos where ID>=@Start and ID<=@End";
                var tmp = conn.Query<Motto>(sql, new
                {
                    Start = start,
                    End = end
                });

                result = tmp.AsList();

            /*
            motto.ID = reader.GetInt64(0);
            motto.Score = reader.GetDouble(1);
            motto.UID = reader.GetString(2);
            motto.Up = reader.GetInt32(3);
            motto.Down = reader.GetInt32(4);
            motto.Reviews = reader.GetInt32(5);
            motto.Loves = reader.GetInt32(6);
            motto.RecruitID = reader.GetInt32(7);
            motto.RecruitTitle = reader.GetString(8);
            motto.Content = reader.GetString(9);
            motto.AddTime = reader.GetDateTime(10);
    
             */
            }
            return result;
        }

        public List<Motto> GetMottosByDay(DateTime theDay, int pIndex, int pSize)
        {
            var result = new List<Motto>();
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = conn.Query<Motto>(@"select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime from Mottos 
                        where TheDay=@TheDay order by score desc limit @Start,@Take",
                    new
                    {
                        TheDay = Utils.GetTheDay(theDay),
                        Start = (pIndex - 1)*pSize,
                        Take = pSize
                    });

                result = tmp.AsList();                
            }

            return result;
        }

        public async Task<List<Motto>> GetMottosByDayAsync(DateTime theDay, int pIndex, int pSize)
        {
            var result = new List<Motto>();            

            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Motto>(@"select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime from Mottos 
                        where TheDay=@TheDay order by score desc limit @Start,@Take",
                       new
                       {
                           TheDay = Utils.GetTheDay(theDay),
                           Start = (pIndex - 1) * pSize,
                           Take = pSize
                       });

                result = tmp.AsList();
            }

            return result;
        }

        public async Task<List<Motto>> GetMottosByCollectionAsync(long cID, int pIndex, int pSize)
        {
            var result = new List<Motto>(pSize);

            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select tm.* from Mottos tm, CollectionMottos tc where tm.ID=tc.MID and tc.CID=@CID 
                    order by tc.AddTime desc limit @Skip,@Take";

                var tmp = await conn.QueryAsync<Motto>(sql, new
                {
                    CID = cID,
                    Skip = (pIndex - 1) * pSize,
                    Take = pSize
                });

                result = tmp.AsList();
            }

            return result;
        }

        public async Task<List<Motto>> GetMottosByUserAsync(string uID, int pIndex, int pSize, bool skipEvaluatingMottos = false)
        {
            string sql = null;
            if (skipEvaluatingMottos)
            {
                sql = @"select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime 
                    from Mottos where UID=@UID and AddTime<@PStart order by id desc limit @Skip,@Take";
            }
            else
            {                
                sql = @"select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime 
                    from Mottos where UID=@UID order by id desc limit @Skip,@Take";
            }

            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Motto>(sql,
                    new
                    {
                        UID = uID,
                        PStart = DateTime.Now.AddDays(-7),
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();                
            }
            
        }

        public async Task<List<Review>> GetReviewsByMottoId(long mID, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                string sql = "select ID,MID,UID,Content,Up,Down,AddTime from Reviews where MID=@MID order by id desc limit @Skip,@Take";

                var tmp = await conn.QueryAsync<Review>(sql,
                    new {
                        MID=mID,
                        Skip = (pIndex-1)*pSize,
                        Take = pSize
                    });

                return tmp.AsList();               
            }
        }

        public async Task<List<Motto>> GetMottosByRecruitAsync(int rID, int pIndex, int pSize)
        {
            var result = new List<Motto>(pSize);
            string sql = "select ID,Score,UID,Up,Down,Reviews,Loves,RecruitID,RecruitTitle,Content,AddTime from Mottos where RecruitID>@RID limit @Skip,@Take";

            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Motto>(sql,
                    new {
                        RID = rID,
                        Skip = (pIndex-1)*pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        public async Task<List<Vote>> GetVotesByMottoAsync(long mID, int pIndex, int pSize)
        {            
            using (var conn = _connProvider.GetConnection())
            {
                string sql = "select * from Votes where MID=@MID order by Id desc limit @Skip, @Take";

                var tmp = await conn.QueryAsync<Vote>(sql,
                    new
                    {
                        MID = mID,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        private Vote GetVoteFromReader(SqlDataReader reader)
        {
            var vote = new Vote();
            vote.MID = reader.GetInt64(0);
            vote.ID = reader.GetInt64(1);
            vote.UID = reader.GetString(2);            
            vote.Support = reader.GetInt32(3);
            vote.Oppose = reader.GetInt32(4);
            vote.VoteTime = reader.GetDateTime(5);

            return vote;
        }

        private Motto GetMottoFromReader(SqlDataReader reader)
        {
            var motto = new Motto();

            motto.ID = reader.GetInt64(0);
            motto.Score = reader.GetDouble(1);
            motto.UID = reader.GetString(2);
            motto.Up = reader.GetInt32(3);
            motto.Down = reader.GetInt32(4);
            motto.Reviews = reader.GetInt32(5);
            motto.Loves = reader.GetInt32(6);
            motto.RecruitID = reader.GetInt32(7);
            motto.RecruitTitle = reader.GetString(8);
            motto.Content = reader.GetString(9);
            motto.AddTime = reader.GetDateTime(10);

            return motto;
        }

        public List<Vote> GetUserVotes(string uid, int theDay)
        {
            var result = new List<Vote>();
            string sql = "select MID,ID,UID,Support,Oppose,VoteTime from Votes where UID=@UID and TheDay>=@TheDay limit 0,10000";
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = conn.Query<Vote>(sql,
                    new
                    {
                        UID = uid,
                        TheDay = theDay
                    });

                return tmp.AsList();
            }
        }

        public List<long> GetUserLovedMottoIds(string uid, int theDay)
        {
            string sql = "select MID from LoveMottos where UID=@UID and TheDay=@theday limit 0,10000";
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = conn.Query<long>(sql, new
                {
                    UID = uid,
                    TheDay = theDay
                });

                return tmp.AsList();
            }
        }

        public async Task<List<long>> GetVotedReviewIdsByUserAndMottoIdAsync(string uid, long mid)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = "select ReviewID from ReviewVotes where MID=@MID and UID =@UID";

                var tmp = await conn.QueryAsync<long>(sql,
                    new
                    {
                        MID = mid,
                        UID = uid
                    });

                return tmp.AsList();
            }
        }

        public async Task<List<Motto>> GetUserLovedMottosAsync(string uID, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select tm.* from Mottos tm, LoveMottos tl where tm.ID = tl.MID and tl.UID=@UID
                        order by tl.LoveTime desc limit @Skip,@Take";
                var tmp = await conn.QueryAsync<Motto>(sql, new
                {
                    UID = uID,
                    Skip = (pIndex - 1) * pSize,
                    Take = pSize
                });

                return tmp.AsList();
            }
        }
    }
}
