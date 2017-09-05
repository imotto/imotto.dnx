using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IGiftRepo:IRepository
    {
        //Task<List<Gift>> GetGiftsAsync(int pIndex, int pSize);
        //Task<List<GiftExchange>> GetGiftExchangesAsync(int giftId, int pIndex, int pSize, int status = -1);
        //Task<List<GiftExchange>> GetUserExchangesAsync(string userId, int pIndex, int pSize);
        //Task<List<Gift>> GetGiftsByVendorAsync(string userId, int status, int pIndex, int pSize);
        //Task<DeliverResult> DeliverGiftAsync(string userId, int giftId, long exchangeId, string exCode);

        ////Task<PrepareExchangeResult> PrepareExchange(string userId, int giftId);

        //Task<ExchangeResult> DoExchangeAsync(string userId, int giftId, long reqInfoId, int amount);
        //Task<int> ReceiveGiftAsync(string userId, long exchangeId);
        //Task<int> ReviewGiftAsync(string userId, int giftId, long exchangeId, double rate, string comment);

        Task<List<Award>> GetAwardsAsync(string uid, int pIndex, int pSize);
        Task<int> ReceiveAwardAsync(string userId, int awardId);
        Task<int> SetAwardAddressAsync(int awardId, string userId, long addrId);
        Task<List<Awardee>> GetAwardeesAsync(int awardId);

        Task<SpotLight> GetAwardSpotlightAsync(int theMonth);
        Task<SpotLight> GetAwardeeSpotligthAsync(int theMonth);


    }
}
