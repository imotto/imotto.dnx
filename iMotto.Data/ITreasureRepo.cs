using iMotto.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace iMotto.Data
{
    public interface ICollectionRepo:IRepository
    {
        Task<int> AddCollectionAsync(Collection t);

        Task<int> AddLoveCollectionAsync(LoveCollection lt);

        Task<int> RemoveLoveCollectionAsync(LoveCollection lt);

        Task<int> AddCollectionMottoAsync(CollectionMotto tm);

        Task<int> RemoveCollectionMottoAsync(CollectionMotto tm);

        Task<List<Collection>> GetCollectionsAsync(int pIndex, int pSize);

        Task<List<Collection>> GetCollectionsByTagAsync(string tag, int pIndex, int pSize);

        Task<List<Collection>> GetUserLovedCollectionsAsync(string uid, int pIndex, int pSize);

        Task<List<Tag>> GetTagsAsync(int pIndex, int pSize);

        Task<List<Collection>> GetCollectionsByUserAsync(string uID, int pIndex, int pSize);

        Task<int> UpdateCollectionAsync(string userId, long cID, string title, string summary);
    }
}
