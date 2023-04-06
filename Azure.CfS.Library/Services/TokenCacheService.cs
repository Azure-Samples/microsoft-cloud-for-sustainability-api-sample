using Azure.CfS.Library.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Azure.CfS.Library.Services
{
    internal class TokenCacheService : ITokenCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public TokenCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheTokenAsync(string cacheKey, string token, TimeSpan timeToLive)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            
            await _distributedCache.SetStringAsync(cacheKey, token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string?> GetCachedTokenAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
