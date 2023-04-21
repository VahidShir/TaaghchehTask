using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class InMemoryCacheGetBookInfoService : AbstractGetBookInfoServiceHandler, IInMemoryCacheGetBookInfoService
{
    private readonly IMemoryCache _memoryCache;
    private TaaghchehSettings _taaghchehSettings;

    public InMemoryCacheGetBookInfoService(IMemoryCache memoryCache, IOptions<TaaghchehSettings> options)
    {
        _memoryCache = memoryCache;
        _taaghchehSettings = options.Value;
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        BookInfo result = null;

        bool cacheExists = _memoryCache.TryGetValue<BookInfo>(bookInfo, out var cachedValue);

        if (cacheExists)
        {
            result = cachedValue;
        }
        else if (_nextService is not null)
        {
            var valueFromNextService = await _nextService.GetBookInfoAsync(bookInfo);

            var expirationTimeSpan = TimeSpan.FromSeconds(_taaghchehSettings.InMemoryCachExpirationTimeInSeconds);

            _memoryCache.Set(bookInfo, valueFromNextService, expirationTimeSpan);

            result = valueFromNextService;
        }

        return result;
    }
}