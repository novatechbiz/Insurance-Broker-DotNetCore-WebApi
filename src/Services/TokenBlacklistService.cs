using Microsoft.Extensions.Caching.Memory;

namespace InsuraNova.Services
{
    public interface ITokenBlacklistService
    {
        Task BlacklistTokenAsync(string jti, DateTime expires);
        Task<bool> IsTokenBlacklistedAsync(string jti);
    }

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IMemoryCache _memoryCache;

        public TokenBlacklistService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task BlacklistTokenAsync(string jti, DateTime expires)
        {
            var expiryTimeSpan = expires - DateTime.UtcNow;

            if (expiryTimeSpan <= TimeSpan.Zero)
            {
                expiryTimeSpan = TimeSpan.FromMinutes(5);
            }

            // Store the token in the memory cache with an expiry time
            _memoryCache.Set($"blacklist:{jti}", true, expiryTimeSpan);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            // Check if the token is blacklisted in memory cache
            return _memoryCache.TryGetValue($"blacklist:{jti}", out _);
        }
    }
}
