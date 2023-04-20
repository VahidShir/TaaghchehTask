using Microsoft.Extensions.Options;

using System.Net.Http.Json;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class WebApiGetBookInfoService : IWebApiGetBookInfoService
{
    private readonly HttpClient _httpClient;
    private TaaghchehSettings _taaghchehSettings;

    public WebApiGetBookInfoService(IHttpClientFactory httpClientFactory, IOptions<TaaghchehSettings> options)
    {
        _taaghchehSettings = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(_taaghchehSettings.GetBookInfoApiEndpoint);
    }

    public async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        BookInfo result = await _httpClient.GetFromJsonAsync<BookInfo>(bookInfo.ToString());

        return result;
    }
}