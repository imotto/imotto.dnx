using iMotto.Data.Entities;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace iMotto.Data.Dapper
{

    public class UserRepo : IUserRepo
    {
        private readonly IConnectionProvider _connProvider;

        public UserRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }

        public async Task<int> InsertAsync(IdentityUser user, string inviteCode)
        {
            var rowAffected = 0;
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    //不使用邀请制
                    //var cmd = conn.CreateCommand();
                    //cmd.CommandText = @"update T_InViteCode set CodeState=@Status,UseUid=@UseUid,UseTime=@UseTime where
                    //Code=@Code and CodeState=@CStatus";
                    //cmd.Transaction = tran;
                    //cmd.Parameters.AddWithValue("@Status", InviteCode.STATUS_USED);
                    //cmd.Parameters.AddWithValue("@UseUid", user.Id);
                    //cmd.Parameters.AddWithValue("@UseTime", DateTime.Now);
                    //cmd.Parameters.AddWithValue("@Code", inviteCode);
                    //cmd.Parameters.AddWithValue("@CStatus", InviteCode.STATUS_NEW);

                    //rowAffected = cmd.ExecuteNonQuery();

                    //if (rowAffected > 0)
                    //{
                    var sql = @"Insert into Users (Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,
                        TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,DisplayName,thumb) 
                        Values(@Id,@Email,@EmailConfirmed,@PasswordHash,@SecurityStamp,@PhoneNumber,@PhoneNumberConfirmed,
                        @TwoFactorAuthEnabled,@LockoutEndDate,@LockoutEnabled,@AccessFailedCount,@UserName,@DisplayName,@thumb)";

                    rowAffected = await conn.ExecuteAsync(sql, user, tran);

                    await conn.ExecuteAsync(@"Insert into UserStatistics (UID) Values (@Uid)",
                        new
                        {
                            UID = user.Id
                        }, tran);

                    tran.Commit();
                    //}
                    //else
                    //{
                    //    tran.Rollback();
                    //}
                }
                catch (Exception)
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

        public async Task<int> DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
            //using (var conn = _connProvider.GetConnection())
            //{
            //    return await conn.ExecuteAsync("Delete from Users where Id=@ID",
            //        new
            //        {
            //            ID=user.Id
            //        });
            //}
        }

        public async Task<IdentityUser> GetByIdAsync(string uid)
        {
            throw new NotImplementedException();
            //IdentityUser user = null;
            //using (var reader = await ExecuteReaderAsync(ConnStr, @"Select Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,
            //        TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName from Users
            //        where Id=@id", new SqlParameter("@id", uid)))
            //{
            //    if (reader.Read())
            //    {
            //        user = GetIdentityUserFromReader(reader);
            //    }
            //}

            //return user;
        }

        public async Task<IdentityUser> GetByNameAsync(string userName)
        {
            throw new NotImplementedException();
            //IdentityUser user = null;
            //using (var reader = await ExecuteReaderAsync(ConnStr, @"Select Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,
            //        TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName from Users
            //        where UserName=@uName", new SqlParameter("@uName", userName)))
            //{
            //    if (reader.Read())
            //    {
            //        user = GetIdentityUserFromReader(reader);
            //    }
            //}

            //return user;
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            User user = null;
            using (var conn = _connProvider.GetConnection())
            {
                using (var reader = await conn.ExecuteReaderAsync(@"select u.Id,u.UserName,u.DisplayName,u.Thumb,t.Mottos,t.Collections,t.Reviews,t.Votes,t.Followers,t.Follows,
                t.LovedMottos,t.LovedCollections,t.Balance,t.Revenue,t.Recruits,t.Reviews,t.Bans,u.PasswordHash,u.Sex 
                from Users u,UserStatistics t where u.ID=t.UID and PhoneNumber=@Phone",
                   new
                   {
                       Phone = phoneNumber
                   }))
                {
                    if (reader.Read())
                    {
                        user = GetUserFromReader(reader);
                        user.Password = reader["PasswordHash"].ToString();
                        user.Sex = (int)reader["Sex"];
                    }
                }
            }

            return user;
        }

        public async Task<IdentityUser> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
            //IdentityUser user = null;
            //using (var reader = await ExecuteReaderAsync(ConnStr, @"Select Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,
            //        TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName from Users
            //        where Email=@email", new SqlParameter("@email", email)))
            //{
            //    if (reader.Read())
            //    {
            //        user = GetIdentityUserFromReader(reader);
            //    }
            //}

            //return user;
        }

        public async Task<int> UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
            //return await ExecuteNonQueryAsync(ConnStr, @"Update Users SET Email=@Email,EmailConfirmed=@EmailConfirmed,PasswordHash=@PasswordHash,SecurityStamp=@SecurityStamp,PhoneNumber=@PhoneNumber,
            //        PhoneNumberConfirmed=@PhoneNumberConfirmed,TwoFactorEnabled=@TwoFactorAuthEnabled,LockoutEndDateUtc=@LockoutEndDate,LockoutEnabled=@LockoutEnabled,
            //        AccessFailedCount=@AccessFailedCount,UserName=@UserName WHERE Id=@Id",
            //    new SqlParameter("@Email", user.Email),
            //    new SqlParameter("@EmailConfirmed", user.EmailConfirmed),
            //    new SqlParameter("@PasswordHash", user.PasswordHash),
            //    new SqlParameter("@SecurityStamp", user.SecurityStamp),
            //    new SqlParameter("@PhoneNumber", user.PhoneNumber),
            //    new SqlParameter("@PhoneNumberConfirmed", user.PhoneNumberConfirmed),
            //    new SqlParameter("@TwoFactorAuthEnabled", user.TwoFactorAuthEnabled),
            //    new SqlParameter("@LockoutEndDate", user.LockoutEndDate),
            //    new SqlParameter("@LockoutEnabled", user.LockoutEnabled),
            //    new SqlParameter("@AccessFailedCount", user.AccessFailedCount),
            //    new SqlParameter("@UserName", user.UserName),
            //    new SqlParameter("@Id", user.Id));
        }

        public async Task<int> UpdateUserPasswordAsync(string userId, string oHashPassword, string nHashPassword)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Users set PasswordHash=@NewPass where Id=@Id and PasswordHash=@OldPass",
                    new
                    {
                        NewPass = nHashPassword,
                        Id = userId,
                        OldPass = oHashPassword
                    });
            }
        }


        public async Task<int> ResetPasswordAsync(string mobile, string hashedPassword)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Users set PasswordHash=@NewPass where PhoneNumber=@Mobile",
                    new
                    {
                        NewPass = hashedPassword,
                        Mobile = mobile
                    });
            }
        }

        public async Task<int> UpdateUserNameAsync(string userId, string userName)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Users set DisplayName=@UserName,UserName=@UserName where Id=@Id",
                    new
                    {
                        Id = userId,
                        UserName = userName
                    });
            }
        }

        public async Task<int> UpdateUserThumbAsync(string userId, string thumb)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Users set Thumb=@Thumb where Id=@Id",
                    new
                    {
                        Thumb = thumb,
                        Id = userId
                    });
            }
        }

        public async Task<int> UpdateUserSexAsync(string userId, int sex)
        {
            using (var conn = _connProvider.GetConnection())
            {
                return await conn.ExecuteAsync("update Users set Sex=@Sex where Id=@Id",
                    new
                    {
                        Sex = sex,
                        Id = userId
                    });
            }
        }

        public async Task<List<User>> GetUsersAsync(List<string> userIds)
        {
            throw new NotImplementedException();
            //var sql = @"select u.ID,u.UserName,u.DisplayName,u.Thumb,t.Mottos,t.Collections,t.Reviews,t.Votes,t.Followers,t.Follows,
            //    t.LovedMottos,t.LovedCollections,t.Balance,t.Revenue,t.Recruits,t.Reviews,t.Bans 
            //    from Users u,UserStatistics t where u.ID=t.UID and u.Id in({0})";

            //var result = new List<User>();

            //var parameters = Helper.BuildDynamicParameters<string>(ref sql, userIds);

            //using (var reader = await ExecuteReaderAsync(ConnStr, sql, parameters.ToArray()))
            //{
            //    while (reader.Read())
            //    {
            //        result.Add(GetUserFromReader(reader));
            //    }
            //}

            //return result;
        }

        public User GetUserById(string uid)
        {
            var sql = @"select u.ID,u.UserName,u.DisplayName,u.Thumb,t.Mottos,t.Collections,t.Reviews,t.Votes,t.Followers,t.Follows,
                t.LovedMottos,t.LovedCollections,t.Balance,t.Revenue,t.Recruits,t.Reviews,t.Bans,u.Sex
                from Users u,UserStatistics t where u.ID=t.UID and u.Id=@uid";

            User user = null;
            using (var conn = _connProvider.GetConnection())
            {
                using (var reader = conn.ExecuteReader(sql, new { UID = uid }))
                {
                    if (reader.Read())
                    {
                        user = GetUserFromReader(reader);
                        user.Sex = (int)reader["Sex"];
                    }
                }
            }

            return user;
        }

        private User GetUserFromReader(IDataReader reader)
        {
            var user = new User();
            user.Id = reader.GetString(0);
            user.UserName = reader.GetString(1);
            user.DisplayName = reader.GetString(2);
            user.Thumb = reader.GetString(3);

            user.Statistics.Mottos = reader.GetInt32(4);
            user.Statistics.Collections = reader.GetInt32(5);
            user.Statistics.Reviews = reader.GetInt32(6);
            user.Statistics.Votes = reader.GetInt32(7);
            user.Statistics.Followers = reader.GetInt32(8);
            user.Statistics.Follows = reader.GetInt32(9);
            user.Statistics.LovedMottos = reader.GetInt32(10);
            user.Statistics.LovedCollections = reader.GetInt32(11);

            user.Statistics.Balance = reader.GetInt32(12);
            user.Statistics.Revenue = reader.GetInt32(13);
            user.Statistics.Recruits = reader.GetInt32(14);
            user.Statistics.Reviews = reader.GetInt32(15);
            user.Statistics.Bans = reader.GetInt32(16);


            return user;
        }

        public async Task<List<RelatedUser>> GetUserBansAsync(string userId, int pIndex, int pSize)
        {
            var result = new List<RelatedUser>();

            //string sql = @"select t.id,t.UserName,t.DisplayName,t.Thumb,t.Sex, CAST(1 as bit) as IsMutual,t.BanTime,ts.Mottos,ts.Revenue,ts.Follows,ts.Followers from (
            //    select tu.id, tu.UserName, tu.DisplayName, tu.Thumb, tu.Sex,tf.BanTime, ROW_NUMBER() over(order by tf.BanTime desc) as rn
            //     from Bans tf, T_Users tu where tf.TUID = tu.Id and tf.SUID = @UID) t, UserStatistics ts where t.Id = ts.UID and t.rn between @start and @end";
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select t.id,t.UserName,t.DisplayName,t.Thumb,t.Sex, CAST(1 as bit) as IsMutual,t.BanTime,ts.Mottos,ts.Revenue,ts.Follows,ts.Followers from (
                    select tu.id, tu.UserName, tu.DisplayName, tu.Thumb, tu.Sex,tf.BanTime from Bans tf, Users tu where tf.TUID = tu.Id and tf.SUID = @UID) t,
                    Userstatistics ts where t.Id = ts.UID limit @Skip,@Take";

                using (var reader = await conn.ExecuteReaderAsync(sql, new
                {
                    UID = userId,
                    Skip = (pIndex - 1) * pSize,
                    Take = pSize
                }))
                {
                    while (reader.Read())
                    {
                        RelatedUser user = GetRelatedUserFromReader(reader);
                        result.Add(user);
                    }
                }
            }

            return result;
        }

        public async Task<List<RelatedUser>> GetFollowerAsync(string uID, int pIndex, int pSize)
        {
            var result = new List<RelatedUser>();

            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select t.id,t.UserName,t.DisplayName,t.Thumb,t.Sex,t.isMutual,t.FollowTime,ts.Mottos,ts.Revenue,ts.Follows,ts.Followers from (
                select tu.id, tu.UserName, tu.DisplayName, tu.Thumb, tu.Sex, tf.IsMutual,tf.FollowTime from Follows tf, Users tu where tf.SUID = tu.Id and tf.TUID=@UID) t,
                UserStatistics ts where t.Id=ts.UID limit @Skip,@Take";

                using (var reader = await conn.ExecuteReaderAsync(sql,
                    new
                    {
                        UID = uID,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    }))
                {
                    while (reader.Read())
                    {
                        RelatedUser user = GetRelatedUserFromReader(reader);
                        result.Add(user);
                    }
                }
            }

            return result;
        }

        private static RelatedUser GetRelatedUserFromReader(IDataReader reader)
        {
            var user = new RelatedUser();
            user.ID = reader.GetString(0);
            user.UserName = reader.GetString(1);
            user.DisplayName = reader.GetString(2);
            user.Thumb = reader.GetString(3);
            user.Sex = reader.GetInt32(4);
            user.IsMutual = reader.GetBoolean(5);
            user.FollowTime = reader.GetDateTime(6);
            user.Mottos = reader.GetInt32(7);
            user.Revenue = reader.GetInt32(8);
            user.Follows = reader.GetInt32(9);
            user.Followers = reader.GetInt32(10);
            return user;
        }

        public async Task<List<RelatedUser>> GetFollowAsync(string uID, int pIndex, int pSize)
        {
            var result = new List<RelatedUser>();

            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select t.id,t.UserName,t.DisplayName,t.Thumb,t.Sex,t.isMutual,t.FollowTime,ts.Mottos,ts.Revenue,ts.Follows,ts.Followers from (
                    select tu.id, tu.UserName, tu.DisplayName, tu.Thumb, tu.Sex, tf.IsMutual,tf.FollowTime from Follows tf,Users tu where tf.TUID = tu.Id and tf.SUID=@uid) t,
                    UserStatistics ts where t.Id=ts.UID limit @Skip,@Take";

                using (var reader = await conn.ExecuteReaderAsync(sql,
                    new
                    {
                        UID = uID,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    }))
                {
                    while (reader.Read())
                    {
                        RelatedUser user = GetRelatedUserFromReader(reader);
                        result.Add(user);
                    }
                }
            }

            return result;
        }


        private IdentityUser GetIdentityUserFromReader(SqlDataReader reader)
        {
            var user = new IdentityUser();

            user.Id = reader.GetString(0);
            user.Email = reader.GetString(1);
            user.EmailConfirmed = reader.GetBoolean(2);
            user.PasswordHash = reader.GetString(3);
            user.SecurityStamp = reader.GetString(4);
            user.PhoneNumber = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
            user.PhoneNumberConfirmed = reader.GetBoolean(6);
            user.TwoFactorAuthEnabled = reader.GetBoolean(7);
            user.LockoutEndDate = reader.IsDBNull(8) ? new DateTime(1970, 1, 1) : reader.GetDateTime(8);
            user.LockoutEnabled = reader.GetBoolean(9);
            user.AccessFailedCount = reader.GetInt32(10);
            user.UserName = reader.GetString(11);


            return user;

        }

        public async Task<List<BillRecord>> GetUserBillRecordsAsync(string userId, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select ID, ChangeType, ChangeAmount, ChangeTime, Summary from UserRevenueRecords 
                    where uid=@UID order by ID desc limit @Skip,@Take";

                var tmp = await conn.QueryAsync<BillRecord>(sql,
                    new
                    {
                        UID = userId,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        public async Task<List<ScoreRecord>> GetUserScoreRecordAsync(string userId, int pIndex, int pSize)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select TheDay, Mottos, Revenue, Score from UserRevenues where uid=@UID order by TheDay desc limit @Skip,@Take";
                var tmp = await conn.QueryAsync<ScoreRecord>(sql,
                    new
                    {
                        UID = userId,
                        Skip = (pIndex - 1) * pSize,
                        Take = pSize
                    });

                return tmp.AsList();
            }
        }

        public async Task<int> AddAddressAsync(UserAddress addr)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"INSERT INTO UserAddresses (UID,Province,City,District,Address,Zip,Contact,Mobile,IsDefault) 
                    VALUES(@UID, @Province, @City, @District, @Address, @Zip, @Contact, @Mobile, @IsDefault);
                    select @@identity";

                var id = await conn.QueryFirstOrDefaultAsync<long>(sql, addr);

                if (id != 0)
                {
                    addr.ID = id;
                    return 1;
                }

                return 0;
            }
        }

        public async Task<int> AddRelAccountAsync(RelAccount account)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"INSERT INTO RelAccounts (UID,Platform,AccountNo,AccountName)
                    VALUES (@UID,@Platform,@AccountNo,@AccountName);
                    select @@IDENTITY";

                var id = await conn.QueryFirstOrDefaultAsync<long>(sql, account);
                if (id != 0)
                {
                    account.ID = id;
                    return 1;
                }

                return 0;
            }
        }

        public async Task<int> SetDefaultAddressAsync(string userId, long addrId)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var tran = conn.BeginTransaction();
                try
                {
                    await conn.ExecuteAsync(@"update T_UserAddresses set IsDefault=0 where UID=@UserId and IsDefault=1",
                        new { UserId = userId }, tran);

                    var rowAffected = await conn.ExecuteAsync("update T_UserAddresses set IsDefault=1 where UID=@UserId and ID=@Aid",
                        new
                        {
                            UserId = userId,
                            Aid = addrId
                        }, tran);

                    tran.Commit();

                    return rowAffected;
                }
                catch
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch
                    {
                        //ignore
                    }

                    throw;
                }
            }
        }

        public async Task<List<UserAddress>> GetUserAddressesAsync(string userId)
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select ID,UID,Province,City,District,Address,Zip,Contact,Mobile,IsDefault 
                    from UserAddresses where UID=@UserId order by id desc";

                var tmp = await conn.QueryAsync<UserAddress>(sql,
                    new
                    {
                        UserId = userId
                    });

                return tmp.AsList();
            }
        }

        public async Task<List<RelAccount>> GetUserRelAccountsAsync(string userId, int type = 0)
        {
            throw new NotImplementedException();
            //var result = new List<RelAccount>();
            //using (var conn = _connProvider.GetConnection())
            //{
            //    await conn.OpenAsync();
            //    var cmd = conn.CreateCommand();
            //    string sql;
            //    if (type == 0)
            //    {
            //        sql = @"select [ID],[UID],[Platform],[AccountNo],[AccountName] 
            //        from T_RelAccounts where UID=@userId order by id desc";
            //    }
            //    else {
            //        sql = @"select [ID],[UID],[Platform],[AccountNo],[AccountName] 
            //        from T_RelAccounts where UID=@userId and Platform=@platform order by id desc";
            //    }

            //    cmd.CommandText = sql;
            //    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.VarChar, 32) { Value = userId });
            //    if (type > 0)
            //    {
            //        cmd.Parameters.Add(new SqlParameter("@platform", SqlDbType.Int) { Value = type });
            //    }

            //    using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (reader.Read())
            //        {
            //            var account = new RelAccount();
            //            account.ID = reader.GetInt64(0);
            //            account.UID = reader.GetString(1);
            //            account.Platform = reader.GetInt32(2);
            //            account.AccountNo = reader.GetString(3);
            //            account.AccountName = reader.GetString(4);

            //            result.Add(account);
            //        }
            //    }
            //}

            //return result;
        }

        public async Task<List<UserAddress>> BatchGetUserAddressesAsync(IEnumerable<long> ids)
        {
            throw new NotImplementedException();
            //List<string> pnames = new List<string>();
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //int idx = 0;
            //foreach (var item in ids)
            //{
            //    var paramName = "@p" + idx;
            //    pnames.Add(paramName);
            //    parameters.Add(new SqlParameter(paramName, SqlDbType.BigInt) { Value = item });
            //    idx++;
            //}

            //var result = new List<UserAddress>();
            //using (var conn = _connProvider.GetConnection())
            //{
            //    await conn.OpenAsync();
            //    var cmd = conn.CreateCommand();
            //    cmd.CommandText = @"select [ID],[UID],[Province],[City],[District],[Address],[Zip],[Contact],[Mobile],[IsDefault] 
            //        from T_UserAddresses where ID in("+ string.Join(",", pnames) +")";

            //    cmd.Parameters.AddRange(parameters.ToArray());

            //    using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (reader.Read())
            //        {
            //            var addr = new UserAddress();
            //            addr.ID = reader.GetInt64(0);
            //            addr.UID = reader.GetString(1);
            //            addr.Province = reader.GetString(2);
            //            addr.City = reader.GetString(3);
            //            addr.District = reader.GetString(4);
            //            addr.Address = reader.GetString(5);
            //            addr.Zip = reader.GetString(6);
            //            addr.Contact = reader.GetString(7);
            //            addr.Mobile = reader.GetString(8);
            //            addr.IsDefault = reader.GetBoolean(9);

            //            result.Add(addr);
            //        }
            //    }
            //}

            //return result;
        }

        public async Task<List<RelAccount>> BatchGetUserRelAccountsAsync(IEnumerable<long> ids)
        {
            throw new NotImplementedException();

            //List<string> pnames = new List<string>();
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //int idx = 0;
            //foreach (var item in ids)
            //{
            //    var paramName = "@p" + idx;
            //    pnames.Add(paramName);
            //    parameters.Add(new SqlParameter(paramName, SqlDbType.BigInt) { Value = item });
            //    idx++;
            //}

            //var result = new List<RelAccount>();
            //using (var conn = _connProvider.GetConnection())
            //{



            //    await conn.OpenAsync();
            //    var cmd = conn.CreateCommand();
            //    string sql = @"select [ID],[UID],[Platform],[AccountNo],[AccountName] 
            //        from T_RelAccounts where ID in ("+ string.Join(",", pnames) + ") order by id desc";

            //    cmd.CommandText = sql;
            //    cmd.Parameters.AddRange(parameters.ToArray());


            //    using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (reader.Read())
            //        {
            //            var account = new RelAccount();
            //            account.ID = reader.GetInt64(0);
            //            account.UID = reader.GetString(1);
            //            account.Platform = reader.GetInt32(2);
            //            account.AccountNo = reader.GetString(3);
            //            account.AccountName = reader.GetString(4);

            //            result.Add(account);
            //        }
            //    }
            //}

            //return result;
        }

        private async Task<List<UserRank>> ReadTotalRankAsync()
        {
            using (var conn = _connProvider.GetConnection())
            {
                var sql = @"select u.ID as UID, T.Revenue as Score, u.DisplayName as UserName,u.Thumb as UserThumb, u.Sex from Users u, UserStatistics t 
                        where T.UID = U.ID order by t.Revenue desc limit 0,100";
                var tmp = await conn.QueryAsync<UserRank>(sql);

                return tmp.AsList();
            }
        }

        public async Task<List<UserRank>> ReadRankedUsersAsync(int theMonth)
        {
            if (theMonth == 0)
            {
                return await ReadTotalRankAsync();
            }
            else
            {
                using (var conn = _connProvider.GetConnection())
                {
                    var sql = @"select t.UID,T.Score, u.DisplayName as UserName, u.Thumb as UserThumb, u.Sex from UserMonthlyRanks t, Users u
                        where t.UID=u.Id and T.TheMonth=@TheMonth order by T.Score desc";

                    var tmp = await conn.QueryAsync<UserRank>(sql,
                        new
                        {
                            TheMonth = theMonth
                        });


                    return tmp.AsList();
                }
            }
        }

        public async Task<int> GetUserUnReadMsgsAsync(string userId)
        {
            int total = 0;

            using (var conn = _connProvider.GetConnection())
            {
                var sql = "select sum(Msgs) from T_RecentTalk where UID=@UID";

                var result = await conn.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    {
                        UID = userId
                    });

                total += result;

                sql = "select count(*) from T_Notice where UID=@uid and state = 0";
                var result2 = await conn.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    {
                        UID = userId
                    });

                total += result2;

            }

            return total;
        }
    }
}
