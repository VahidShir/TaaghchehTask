using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class InMemoryCacheGetBookInfoService : AbstractGetBookInfoServiceHandler, IInMemoryCacheGetBookInfoService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<InMemoryCacheGetBookInfoService> _logger;
    private TaaghchehSettings _taaghchehSettings;

    public InMemoryCacheGetBookInfoService(IMemoryCache memoryCache, IOptions<TaaghchehSettings> options, ILogger<InMemoryCacheGetBookInfoService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _taaghchehSettings = options.Value;
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookId)
    {
        _logger.LogInformation($"Getting book info. Book id:{bookId}");

        BookInfo result = null;

        bool cacheExists = _memoryCache.TryGetValue<BookInfo>(bookId, out var cachedValue);

        if (cacheExists)
        {
            result = cachedValue;

            _logger.LogInformation($"BookInfo does exist in in-memory cache and returning it.");

        }
        else if (_nextService is not null)
        {
            _logger.LogInformation($"BookInfo doesn't exist in in-memory cache and therefore getting it from next layer.");

            var valueFromNextService = await _nextService.GetBookInfoAsync(bookId);

            if (valueFromNextService is not null)
            {
                _logger.LogInformation($"Successfully got bookInfo from next layer. Returning it and also saving it in in-memory cache.");

                var expirationTimeSpan = TimeSpan.FromSeconds(_taaghchehSettings.InMemoryCachExpirationTimeInSeconds);

                _memoryCache.Set(bookId, valueFromNextService, expirationTimeSpan);

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