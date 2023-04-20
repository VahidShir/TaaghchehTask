using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class InMemoryCacheGetBookInfoService : IInMemoryCacheGetBookInfoService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IWebApiGetBookInfoService _webApiGetBookInfoService;
    private TaaghchehSettings _taaghchehSettings;

    public InMemoryCacheGetBookInfoService(IMemoryCache memoryCache, IWebApiGetBookInfoService webApiGetBookInfoService, IOptions<TaaghchehSettings> options)
    {
        _memoryCache = memoryCache;
        _webApiGetBookInfoService = webApiGetBookInfoService;
        _taaghchehSettings = options.Value;
    }

    public async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(
        bookInfo,
        async cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_taaghchehSettings.InMemoryCachExpirationTimeInSeconds);
            return await _webApiGetBookInfoService.GetBookInfoAsync(bookInfo);
        });

        return cachedValue;
    }
}