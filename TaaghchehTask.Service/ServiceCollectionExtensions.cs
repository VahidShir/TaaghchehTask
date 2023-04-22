using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWTaaghchehervices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<TaaghchehSettings>(configuration.GetSection("TaaghchehTask"));
        services.AddSingleton<IWebApiGetBookInfoService, WebApiGetBookInfoService>();
        services.AddMemoryCache();
        services.AddSingleton<IInMemoryCacheGetBookInfoService, InMemoryCacheGetBookInfoService>();
        AddRedisService(services, configuration);
        services.AddSingleton<IRedisGetBookInfoService, RedisGetBookInfoService>();
        services.AddSingleton<IFileGetBookInfoService, FileGetBookInfoService>();
        services.AddSingleton<IGetBookInfoServiceHandler, InMemoryCacheGetBookInfoService>();
        services.AddSingleton<IGetBookInfoServiceHandler, RedisGetBookInfoService>();
        services.AddSingleton<IGetBookInfoServiceHandler, WebApiGetBookInfoService>();
        services.AddSingleton<IGetBookInfoServiceHandler, FileGetBookInfoService>();
        services.AddSingleton<ILayeredGetBookInfoService, LayeredGetBookInfoService>();

        return services;
    }

    private static void AddRedisService(IServiceCollection services, ConfigurationManager configuration)
    {
        var redisAddress = configuration["TaaghchehTask:RedisAddress"];

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisAddress;
        });
    }
}