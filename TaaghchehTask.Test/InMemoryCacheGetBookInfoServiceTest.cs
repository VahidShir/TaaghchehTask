using AutoFixture;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using System.Text.Json;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Service;

using File = System.IO.File;

namespace TaaghchehTask.Test;

public class InMemoryCacheGetBookInfoServiceTest
{
    private Fixture _fixture;

    public InMemoryCacheGetBookInfoServiceTest()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task WithoutNextServiceHandlerSet_EmptyCache_MustReturnNull()
    {
        //Arrange
        var memoryCacheFake = A.Fake<IMemoryCache>();
        var loggerFake = new NullLogger<InMemoryCacheGetBookInfoService>();
        var optionsFake = A.Fake<IOptions<TaaghchehSettings>>();
        var inMemoryCacheService = new InMemoryCacheGetBookInfoService(memoryCacheFake, optionsFake, loggerFake);
        long bookId = _fixture.Create<long>();

        //Act
        var value = await inMemoryCacheService.GetBookInfoAsync(bookId);

        //Assert
        value.Should().BeNull();
    }

    [Fact]
    public async Task WithoutNextServiceHandlerSet_ExistingCacheValue_MustBeReturned()
    {
        //Arrange
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var loggerFake = new NullLogger<InMemoryCacheGetBookInfoService>();
        var optionsFake = A.Fake<IOptions<TaaghchehSettings>>();
        var inMemoryCacheService = new InMemoryCacheGetBookInfoService(memoryCache, optionsFake, loggerFake);

        var bookInfo = await GetSampleBookInfo();
        long bookId = bookInfo.Book.Id;

        memoryCache.Set(bookId, bookInfo);

        //Act
        BookInfo actualValue = await inMemoryCacheService.GetBookInfoAsync(bookId);

        //Assert
        actualValue.Should().NotBeNull();
        actualValue.Should().Be(bookInfo);
    }

    [Fact]
    public async Task WithNextServiceHandlerIsSetAndNextServiceHandlerHasExistingCachedValue_ExistingCacheValue_MustBeReturned()
    {
        //Arrange
        var redisOptions = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache redisCach = new MemoryDistributedCache(redisOptions);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var loggerFakeInMemoryCache = new NullLogger<InMemoryCacheGetBookInfoService>();
        var loggerFakeRedisCache = new NullLogger<RedisGetBookInfoService>();
        var optionsFake = Options.Create(new TaaghchehSettings { InMemoryCachExpirationTimeInSeconds = 5 });
        var inMemoryCacheService = new InMemoryCacheGetBookInfoService(memoryCache, optionsFake, loggerFakeInMemoryCache);

        var bookInfo = await GetSampleBookInfo();
        long bookId = bookInfo.Book.Id;

        var redisService = A.Fake<RedisGetBookInfoService>(x => x.WithArgumentsForConstructor(new object[] { redisCach, optionsFake, loggerFakeRedisCache }));

        A.CallTo(() => redisService.GetBookInfoAsync(bookId)).Returns(bookInfo);

        //Set next handler layer
        inMemoryCacheService.SetNextLayer(redisService);

        //Act
        BookInfo actualValue = await inMemoryCacheService.GetBookInfoAsync(bookId);

        //Assert
        actualValue.Should().NotBeNull();
        actualValue.Should().Be(bookInfo);
        A.CallTo(() => redisService.GetBookInfoAsync(bookId)).MustHaveHappenedOnceExactly();
    }

    private async Task<BookInfo> GetSampleBookInfo()
    {
        var file = await File.ReadAllTextAsync("sampleBook.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        BookInfo bookInfo = JsonSerializer.Deserialize<BookInfo>(file, options);

        return bookInfo;
    }
}