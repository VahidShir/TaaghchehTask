using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class RedisGetBookInfoService : AbstractGetBookInfoServiceHandler, IRedisGetBookInfoService
{
    private readonly IDistributedCache _cache;
    private TaaghchehSettings _taaghchehSettings;

    public RedisGetBookInfoService(IDistributedCache cache, IOptions<TaaghchehSettings> options)
    {
        _cache = cache;
        _taaghchehSettings = options.Value;
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        BookInfo result = null;

        var cachedValue = await _cache.GetStringAsync(bookInfo.ToString());

        if (cachedValue is not null)
        {
            result = cachedValue.ToBookInfo();
        }
        else if (_nextService is not null)
        {
            var valueFromNextService = await _nextService.GetBookInfoAsync(bookInfo);

            var expirationTimeSpan = TimeSpan.FromSeconds(_taaghchehSettings.InMemoryCachExpirationTimeInSeconds);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTimeSpan
            };

            await _cache.SetStringAsync(bookInfo.ToString(), valueFromNextService.ToString(), options);

            result = valueFromNextService;
        }

        return result;
    }
}