using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class RedisGetBookInfoService : AbstractGetBookInfoServiceHandler, IRedisGetBookInfoService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisGetBookInfoService> _logger;
    private TaaghchehSettings _taaghchehSettings;

    public RedisGetBookInfoService(IDistributedCache cache, IOptions<TaaghchehSettings> options, ILogger<RedisGetBookInfoService> logger)
    {
        _cache = cache;
        _logger = logger;
        _taaghchehSettings = options.Value;
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookId)
    {
        _logger.LogInformation($"Getting book info. Book id:{bookId}");

        BookInfo result = null;

        string cachedValue = await _cache.GetStringAsync(bookId.ToString());

        if (!string.IsNullOrWhiteSpace(cachedValue))
        {
            _logger.LogInformation($"BookInfo does exist in redis cache and returning it.");

            result = cachedValue.ToBookInfo();
        }
        else if (_nextService is not null)
        {
            _logger.LogInformation($"BookInfo doesn't exist in rediscache and therefore getting it from next layer.");

            var valueFromNextService = await _nextService.GetBookInfoAsync(bookId);

            if (valueFromNextService is not null)
            {
                _logger.LogInformation($"Successfully got bookInfo from next layer. Returning it and also saving it in redis cache.");

                var expirationTimeSpan = TimeSpan.FromSeconds(_taaghchehSettings.InMemoryCachExpirationTimeInSeconds);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTimeSpan
                };

                await _cache.SetStringAsync(bookId.ToString(), valueFromNextService.ToString(), options);

                result = valueFromNextService;
            }
            else
            {
                _logger.LogInformation($"couldn't get bookInfo from next layer. Returning null.");
            }
        }

        return result;
    }
}