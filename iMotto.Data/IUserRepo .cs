using iMotto.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iMotto.Data
{
    public interface IUserRepo:IRepository
    {
        Task<int> InsertAsync(IdentityUser user, string inviteCode);

        Task<int> DeleteAsync(IdentityUser user);

        Task<IdentityUser> GetByIdAsync(string uid);

        Task<IdentityUser> GetByNameAsync(string userName);

        Task<IdentityUser> GetByEmailAsync(string email);

        Task<User> GetByPhoneNumberAsync(string phoneNumber);

        Task<List<User>> GetUsersAsync(List<string> uIDS);

        Task<int> GetUserUnReadMsgsAsync(string userId);

        Task<List<UserRank>> ReadRankedUsersAsync(int theMonth);

        Task<List<BillRecord>>  GetUserBillRecordsAsync(string userId, int pIndex, int pSize);

        Task<List<ScoreRecord>> GetUserScoreRecordAsync(string userId, int pIndex, int pSize);

        User GetUserById(string uid);

        Task<int> UpdateAsync(IdentityUser user);

        Task<List<RelatedUser>> GetFollowAsync(string uID, int pIndex, int pSize);
        
        Task<List<RelatedUser>> GetFollowerAsync(string uID, int pIndex, int pSize);

        Task<int> AddAddressAsync(UserAddress addr);

        Task<int> AddRelAccountAsync(RelAccount account);

        Task<int> SetDefaultAddressAsync(string userId, long addrId);
        

        Task<List<UserAddress>> GetUserAddressesAsync(string userId);

        Task<List<UserAddress>> BatchGetUserAddressesAsync(IEnumerable<long> ids);

        Task<List<RelAccount>> GetUserRelAccountsAsync(string userId, int type=0);

        Task<List<RelAccount>> BatchGetUserRelAccountsAsync(IEnumerable<long> ids);

        Task<List<RelatedUser>> GetUserBansAsync(string userId, int pIndex, int pSize);

        Task<int> UpdateUserPasswordAsync(string userId, string oHashPassword, string nHashPassword);

        Task<int> ResetPasswordAsync(string mobile, string hashedPassword);

        Task<int> UpdateUserNameAsync(string userId, string userName);

        Task<int> UpdateUserThumbAsync(string userId, string thumb);

        Task<int> UpdateUserSexAsync(string userId, int sex);

    }
}
