using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace QRCodeAPI.CacheStore
{
    public class InMemoryCacheStore : ICacheStore
    {
        private const string TAG = "[InMemoryCacheStore]";
        
        private readonly IMemoryCache _cache;
        private readonly ILogger<InMemoryCacheStore> _logger;

        public InMemoryCacheStore(IMemoryCache cache, ILogger<InMemoryCacheStore> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        
        public async Task AddAsync<TItem>(ICacheKey key, TItem item)
        {
            _logger.LogTrace($"{TAG} AddAsync called for {key.ToString()}");
            if (item == null)
                return;

            var cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _logger.LogTrace($"{TAG} Adding item to cache.");
            await Task.Run(() => _cache.Set(key, item, cacheOptions));
        }

        public async Task<TItem> GetAsync<TItem>(ICacheKey key) where TItem : class
        {
            _logger.LogTrace($"{TAG} GetAsync called for {key.ToString()}");
            return await Task.Run(() => _cache.Get<TItem>(key));
        }
    }
}