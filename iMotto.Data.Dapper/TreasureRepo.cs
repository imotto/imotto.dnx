using Dapper;
using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class CollectionRepo : ICollectionRepo
    {
        private IConnectionProvider _connProvider;
        public CollectionRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }


        public async Task<int> AddCollectionAsync(Collection t)
        {
            var rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    var id = await conn.QueryFirstOrDefaultAsync<long>(@"insert into Collections (UID,Title,Description,CreateTime,Tags)
                        values(@UID,@Title,@Description,@CreateTime,@Tags);select @@IDENTITY",
                        t, tran);
             
                    if (id != 0)
                    {
                        t.ID = id;

                        await conn.ExecuteAsync("update T_UserStatistics set Collections=Collections+1 where UID=@uid",
                            new
                            {
                                UID = t.UID
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

        public async Task<int> AddLoveCollectionAsync(LoveCollection lt)
        {
            int rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    rowAffected = await conn.ExecuteAsync("insert into T_LoveCollection (UID,LoveTime,CID) values (@UID,@LoveTime,@CID)",
                        lt, tran);

                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update T_UserStatistics set LovedCollections=LovedCollections+1 where UID=@uid",
                            new
                            {
                                UID = lt.UID
                            }, tran);

                        await conn.ExecuteAsync("update Collections set Loves=Loves+1 where ID=@CID",
                            new
                            {
                                CID = lt.CID
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

        public async Task<int> RemoveLoveCollectionAsync(LoveCollection lt)
        {
            int rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {                
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffected = await conn.ExecuteAsync("delete from T_LoveCollection where UID=@UID and CID=@CID",
                        lt, tran);

                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update T_UserStatistics set LovedCollections=LovedCollections-1 where UID=@UID",
                            new {
                                UID = lt.UID
                            }, tran);

                        await conn.ExecuteAsync("update Collections set Loves=Loves-1 where ID=@CID",
                            new {
                                CID = lt.CID
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

        public async Task<int> AddCollectionMottoAsync(CollectionMotto tm)
        {
            int rowAffeted = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffeted = await conn.ExecuteAsync("insert into CollectionMottos (CID,MID,AddTime) values (@CID,@MID,@AddTime)",
                        tm, tran);
                        
                    if (rowAffeted > 0)
                    {
                        await conn.ExecuteAsync("update Collections set Mottos = Mottos+1 where ID=@CID",
                            new
                            {
                                CID = tm.CID
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

            return rowAffeted;
        }

        public async Task<int> RemoveCollectionMottoAsync(CollectionMotto tm)
        {
            var rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    rowAffected = await conn.ExecuteAsync("delete from CollectionMottos where CID=@CID and MID=@MID",
                        tm, tran);                        
                 
                    if (rowAffected > 0)
                    {
                        await conn.ExecuteAsync("update Collections set Mottos = Mottos-1 where ID=@CID",
                            new
                            {
                                CID = tm.CID
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

        public async Task<int> AddTagCollectionAsync(TagCollection tc)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync(@"insert into TagCollections(TagName,CID,CollectionTitle,CollectionScore) 
                    values (@TagName,@CID,@CollectionTitle,@CollectionScore)", tc);
            }

        }

        public async Task<int> AddTagAsync(Tag t)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("insert into T_Tag(Name,Collections) values(@Name,@Collections)", t);
            }
        }

        public async Task<List<Collection>> GetCollectionsAsync(int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Collection>(@"select ID,Score,UID,Title,Description,Mottos,Loves,CreateTime,Tags 
                    from Collections order by Score desc limit @Skip,@Take",
                    new
                    {
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }            
        }

        public async Task<List<Collection>> GetCollectionsByTagAsync(string tag, int pIndex, int pSize)
        {
            throw new NotImplementedException();
            //var result = new List<Collection>(pSize);
            //string sql = @"select t.ID,t.Score,t.UID,t.Title,t.Description,t.Mottos,t.Loves,t.CreateTime,t.Tags from Collections t,T_TagCollection u 
            //    where t.ID=u.CID and u.TagName=@tag order by t.Score desc limit @start,@take";

            //using (var reader = await ExecuteReaderAsync(ConnStr, sql,
            //   new SqlParameter("@tag", tag),
            //   new SqlParameter("@start", (pIndex - 1) * pSize),
            //   new SqlParameter("@take", pSize)))
            //{
            //    while (reader.Read())
            //    {
            //        result.Add(GetCollectionFromReader(reader));
            //    }
            //}

            //return result;
        }

        public async Task<List<Tag>> GetTagsAsync(int pIndex, int pSize)
        {
            throw new NotImplementedException();
            //var result = new List<Tag>(pSize);
            //var sql = "select ID,Name,Collections from T_Tag order by Collections desc limit @start,@take";
            //using (var reader = await ExecuteReaderAsync(ConnStr, sql,
            //    new SqlParameter("@start", (pIndex - 1) * pSize),
            //    new SqlParameter("@take", pSize)))
            //{
            //    while (reader.Read())
            //    {
            //        var tag = new Tag();

            //        tag.ID = reader.GetInt32(0);
            //        tag.Name = reader.GetString(1);
            //        tag.Collections = reader.GetInt32(2);

            //        result.Add(tag);
            //    }
            //}
            //return result;
        }

        public async Task<List<Collection>> GetCollectionsByUserAsync(string uID, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {

                var tmp = await conn.QueryAsync<Collection>(@"select ID,Score,UID,Title,Description,Mottos,Loves,CreateTime,Tags from
                    Collections where UID = @UID order by ID desc limit @Skip,@Take",
                    new
                    {
                        UID = uID,
                        Skip = (pIndex-1)*pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        private Collection GetCollectionFromReader(SqlDataReader reader)
        {
            var collection = new Collection();
            collection.ID = reader.GetInt64(0);
            collection.Score = reader.GetDouble(1);
            collection.UID = reader.GetString(2);
            collection.Title = reader.GetString(3);
            collection.Description = reader.GetString(4);
            collection.Mottos = reader.GetInt32(5);
            collection.Loves = reader.GetInt32(6);
            collection.CreateTime = reader.GetDateTime(7);
            collection.Tags = reader.GetString(8);

            return collection;
        }

        public async Task<List<Collection>> GetUserLovedCollectionsAsync(string uid, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Collection>(@"select ID, Score, UID, Title, Description, Mottos, Loves, CreateTime, Tags 
                    from LoveCollections lc, Collections c where lc.CID =c.ID and lc.UID=@UID
                    order by lc.LoveTime desc limit @Skip,@Take",
                    new
                    {
                        UID = uid,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        public async Task<int> UpdateCollectionAsync(string userId, long cID, string title, string summary)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Collections set Title=@Title,Description=@Description where ID=@CID and UID=@UserId",
                    new
                {
                    Title = title,
                    Description = summary,
                    CID = cID,
                    UserId = userId
                });                
            }
        }
    }
}
