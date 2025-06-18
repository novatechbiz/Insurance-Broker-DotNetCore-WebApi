using StackExchange.Redis;

namespace InsuraNova.Services
{

    public interface ITokenBlacklistService
    {
        Task BlacklistTokenAsync(string jti, DateTime expires);
        Task<bool> IsTokenBlacklistedAsync(string jti);
    }

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IDatabase _redis;

        public TokenBlacklistService(IConnectionMultiplexer radis)
        {
            _redis = radis.GetDatabase();
        }

        public async Task BlacklistTokenAsync(string jti, DateTime expires)
        {
            var expiryTimeSpan = expires - DateTime.UtcNow;

            if (expiryTimeSpan <= TimeSpan.Zero)
            {
                expiryTimeSpan = TimeSpan.FromMinutes(5);
            }

            await _redis.StringSetAsync($"blacklist:{jti}", "true", expiryTimeSpan);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            return await _redis.KeyExistsAsync($"blacklist:{jti}");
        }
    }
}
