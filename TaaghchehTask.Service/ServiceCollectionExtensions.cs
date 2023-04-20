using Microsoft.Extensions.DependencyInjection;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWTaaghchehervices(this IServiceCollection services)
    {
        services.AddSingleton<IWebApiGetBookInfoService, WebApiGetBookInfoService>();

        return services;
    }
}