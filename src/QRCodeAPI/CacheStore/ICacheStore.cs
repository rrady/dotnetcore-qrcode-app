using System.Threading.Tasks;

namespace QRCodeAPI.CacheStore
{
    public interface ICacheStore
    {
        Task AddAsync<TItem>(ICacheKey key, TItem item);
        Task<TItem> GetAsync<TItem>(ICacheKey key) where TItem : class;
    }
}