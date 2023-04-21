using Microsoft.Extensions.Options;

using System.Net.Http.Json;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class WebApiGetBookInfoService : AbstractGetBookInfoServiceHandler, IWebApiGetBookInfoService
{
    private readonly HttpClient _httpClient;
    private TaaghchehSettings _taaghchehSettings;

    public WebApiGetBookInfoService(IHttpClientFactory httpClientFactory, IOptions<TaaghchehSettings> options)
    {
        _taaghchehSettings = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(_taaghchehSettings.GetBookInfoApiEndpoint);
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        BookInfo result = await _httpClient.GetFromJsonAsync<BookInfo>(bookInfo.ToString());

        return result;

        //if (_nextService is not null)
        //{
        //    // add code here in case next service(layer) is added in future

        //}
    }
}