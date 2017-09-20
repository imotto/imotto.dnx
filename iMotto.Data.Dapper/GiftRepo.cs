using Dapper;
using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data.Dapper
{
    public class GiftRepo:IGiftRepo
    {
        private readonly IConnectionProvider _connProvider;

        public GiftRepo(IConnectionProvider connProvider)
        {
            _connProvider = connProvider;
        }
        

        public async Task<List<Award>> GetAwardsAsync(string uid, int pIndex, int pSize)
        {
            List<Award> result = new List<Award>();
            var sql = @"select * from (select * from Awards order by Id desc limit @Start, @Take) ta left join
	                    (select AwardId, Status GainStatus from Awardees where UID=@UID) tb on ta.ID = tb.AwardId";

            if (string.IsNullOrWhiteSpace(uid))
            {
                sql = @"select * from Awards order by id desc limit @Start, @Take";
            }

            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Award>(sql, new
                {
                    Start = (pIndex - 1) * pSize,
                    Take = pIndex * pSize,
                    UID = uid
                });

                result = tmp.AsList();
            }

            return result;
        }

        public async Task<List<Awardee>> GetAwardeesAsync(int awardId)
        {
            List<Awardee> result = new List<Awardee>();
            string sql = @"select ta.UID,ta.Rank,tb.DisplayName UserName,tb.Thumb UserThumb from Awardees ta, Users tb 
                where ta.AwardId = @AwardId and ta.UID = tb.Id order by ta.Rank";

            using (var conn = _connProvider.GetConnection())
            {
                var tmp = await conn.QueryAsync<Awardee>(sql,
                    new
                    {
                        AwardId = awardId
                    });

                result = tmp.AsList();
            }

            return result;
        }

        public async Task<int> ReceiveAwardAsync(string userId, int awardId)
        {
            int rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                rowAffected = await conn.ExecuteAsync("update Awardees set Status = 4 where AwardId = @AwardId and UID = @UID and Status=3",
                    new {
                        AwardId = awardId,
                        UID = userId
                    });
            }

            return rowAffected;
        }

        public async Task<int> SetAwardAddressAsync(int awardId, string userId, long addrId)
        {
            int rowAffected = 0;

            using (var conn = _connProvider.GetConnection())
            {
                rowAffected = await conn.ExecuteAsync("update Awardees set Status=2, AddrId =@AddrId where AwardId = @AwardId and UID=@UID and Status=1",
                    new
                    {
                        AddrId = addrId,
                        AwardId = awardId,
                        UID = userId
                    });
            }

            return rowAffected;
        }

        public async Task<SpotLight> GetAwardSpotlightAsync(int theMonth)
        {
            SpotLight spotlight = null;
            string sql = "select ID, Name, Img, Status, Amount from Awards where ID=@awardId";

            using (var conn =_connProvider.GetConnection())
            {
                var tmp = await conn.QueryFirstOrDefaultAsync<dynamic>(sql,
                    new {
                        AwardId = theMonth
                    });

                if (tmp != null)
                {
                    spotlight = new SpotLight();
                    spotlight.TheMonth = tmp.ID;
                    string name = tmp.Name;
                    spotlight.Img = tmp.Img;
                    int amount = tmp.Amount;
                    spotlight.Info = string.Format("{0}期赛程已开启，本期奖品：【{1}】，共设{2}个获奖名额。小伙伴们，加油哦！", spotlight.TheMonth, name, amount);
                    spotlight.Type = 2;
                }                
            }

            return spotlight;
        }

        public async Task<SpotLight> GetAwardeeSpotligthAsync(int theMonth)
        {
            int lastMonth = theMonth;
            if (theMonth % 100 == 1)
            {
                lastMonth = theMonth - 101 + 12;
            }
            else
            {
                lastMonth = theMonth - 1;
            }

            SpotLight spot = null;
            List<string> users = new List<string>();
           
            using (var conn = _connProvider.GetConnection())
            {

                var tmp = await conn.QueryAsync<string>(@"select tb.DisplayName from Awardees ta,Users tb
                        where ta.UID = tb.id and ta.AwardId=@AwardId order by ta.Rank",
                        new
                        {
                            AwardId = lastMonth
                        });

                users = tmp.AsList();                
            }

            if (users.Count > 0)
            {
                spot = new SpotLight();
                spot.TheMonth = theMonth;
                spot.Type = 1;
                spot.Img = "http://app.imotto.net/pic/winners.jpg";
                spot.Info = string.Format("{0}期赛程已结束，当期积分排行榜已出炉。[{1}]等{2}位小伙伴获得该期奖品。", lastMonth, String.Join("、", users), users.Count);
            }

            return spot;
        }

        public Task<ExchangeResult> DoExchangeAsync(string userId, int giftId, long reqInfoId, int amount)
        {
            throw new NotImplementedException();
        }

        public Task<int> ReceiveGiftAsync(string userId, long exchangeId)
        {
            throw new NotImplementedException();
        }

        public Task<int> ReviewGiftAsync(string userId, int giftId, long exchangeId, double rate, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<List<Gift>> GetGiftsAsync(int pIndex, int pSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<GiftExchange>> GetGiftExchangesAsync(int giftId, int pIndex, int pSize, int status = -1)
        {
            throw new NotImplementedException();
        }

        public Task<List<GiftExchange>> GetUserExchangesAsync(string userId, int pIndex, int pSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<Gift>> GetGiftsByVendorAsync(string userId, int status, int pIndex, int pSize)
        {
            throw new NotImplementedException();
        }

        public Task<DeliverResult> DeliverGiftAsync(string userId, int giftId, long exchangeId, string exCode)
        {
            throw new NotImplementedException();
        }
    }
}
