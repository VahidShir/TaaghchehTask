using Microsoft.Extensions.Caching.Distributed;

using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class RedisGetBookInfoService : IRedisGetBookInfoService
{
    private readonly IDistributedCache _cache;

    public RedisGetBookInfoService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        var cachedValue = await _cache.GetStringAsync(bookInfo.ToString());

        return cachedValue?.ToBookInfo();
    }
}