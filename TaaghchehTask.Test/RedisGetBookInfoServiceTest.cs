using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Service;

namespace TaaghchehTask.Test;

public class RedisGetBookInfoServiceTest
{
    private Fixture _fixture;

    public RedisGetBookInfoServiceTest()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task WithoutNextServiceHandlerSet_EmptyCache_MustReturnNull()
    {
        //Arrange
        var loggerFake = new NullLogger<RedisGetBookInfoService>();
        var optionsFake = A.Fake<IOptions<TaaghchehSettings>>();
        IDistributedCache distributedCacheFake = A.Fake<IDistributedCache>();
        var redisCacheService = new RedisGetBookInfoService(distributedCacheFake, optionsFake, loggerFake);
        long bookId = _fixture.Create<long>();

        //Act
        var value = await redisCacheService.GetBookInfoAsync(bookId);

        //Assert
        value.Should().BeNull();
    }

    [Fact]
    public async Task WithoutNextServiceHandlerSet_ExistingCacheValue_MustBeReturned()
    {
        //Arrange
        //Arrange
        var loggerFake = new NullLogger<RedisGetBookInfoService>();
        var optionsFake = A.Fake<IOptions<TaaghchehSettings>>();
        var redisOptions = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache redisCach = new MemoryDistributedCache(redisOptions);
        var redisCacheService = new RedisGetBookInfoService(redisCach, optionsFake, loggerFake);

        var bookInfo = await Helper.GetSampleBookInfo();
        long bookId = bookInfo.Book.Id;

        var redisCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
        };

        await redisCach.SetStringAsync(bookId.ToString(), bookInfo.ToString(), redisCacheEntryOptions);

        //Act
        BookInfo actualValue = await redisCacheService.GetBookInfoAsync(bookId);

        //Assert
        actualValue.Should().NotBeNull();
        actualValue.Should().BeEquivalentTo(bookInfo);
    }

    [Fact]
    public async Task WithNextServiceHandlerIsSetAndNextServiceHandlerHasExistingCachedValue_ExistingCacheValue_MustBeReturned()
    {
        //Arrange
        var redisOptions = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache redisCach = new MemoryDistributedCache(redisOptions);
        var loggerFakeRedisCache = new NullLogger<RedisGetBookInfoService>();
        var loggerFakeWebApiService = new NullLogger<WebApiGetBookInfoService>();
        var taaghchehOptionsFake = Options.Create(new TaaghchehSettings
        {
            InMemoryCachExpirationTimeInSeconds = 5,
            GetBookInfoApiEndpoint = "https://127.0.0.1"
        });
        var redisCacheService = new RedisGetBookInfoService(redisCach, taaghchehOptionsFake, loggerFakeRedisCache);
        IHttpClientFactory httpClientFactoryFake = A.Fake<IHttpClientFactory>();

        var bookInfo = await Helper.GetSampleBookInfo();
        long bookId = bookInfo.Book.Id;

        var webApiServiceFake = A.Fake<WebApiGetBookInfoService>(x => x.WithArgumentsForConstructor(new object[] { httpClientFactoryFake, taaghchehOptionsFake, loggerFakeWebApiService }));

        A.CallTo(() => webApiServiceFake.GetBookInfoAsync(bookId)).Returns(bookInfo);

        //Set next handler layer
        redisCacheService.SetNextLayer(webApiServiceFake);

        //Act
        BookInfo actualValue = await redisCacheService.GetBookInfoAsync(bookId);

        //Assert
        actualValue.Should().NotBeNull();
        actualValue.Should().Be(bookInfo);
        A.CallTo(() => webApiServiceFake.GetBookInfoAsync(bookId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task WithNextServiceHandlerIsSetAndNextServiceHandlerHasNotExistingCachedValue_ExistingCacheValue_NullMustBeReturned()
    {
        //Arrange
        var redisOptions = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache redisCach = new MemoryDistributedCache(redisOptions);
        var loggerFakeRedisCache = new NullLogger<RedisGetBookInfoService>();
        var loggerFakeWebApiService = new NullLogger<WebApiGetBookInfoService>();
        var taaghchehOptionsFake = Options.Create(new TaaghchehSettings
        {
            InMemoryCachExpirationTimeInSeconds = 5,
            GetBookInfoApiEndpoint = "https://127.0.0.1"
        });
        var redisCacheService = new RedisGetBookInfoService(redisCach, taaghchehOptionsFake, loggerFakeRedisCache);
        IHttpClientFactory httpClientFactoryFake = A.Fake<IHttpClientFactory>();

        var bookInfo = await Helper.GetSampleBookInfo();
        long bookId = bookInfo.Book.Id;

        var webApiServiceFake = A.Fake<WebApiGetBookInfoService>(x => x.WithArgumentsForConstructor(new object[] { httpClientFactoryFake, taaghchehOptionsFake, loggerFakeWebApiService }));

        BookInfo nullBookInfo = null;
        A.CallTo(() => webApiServiceFake.GetBookInfoAsync(bookId)).Returns(nullBookInfo);

        //Set next handler layer
        redisCacheService.SetNextLayer(webApiServiceFake);

        //Act
        BookInfo actualValue = await redisCacheService.GetBookInfoAsync(bookId);

        //Assert
        actualValue.Should().BeNull();
        A.CallTo(() => webApiServiceFake.GetBookInfoAsync(bookId)).MustHaveHappenedOnceExactly();
    }
}