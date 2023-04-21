using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Net.Http.Json;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class WebApiGetBookInfoService : AbstractGetBookInfoServiceHandler, IWebApiGetBookInfoService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WebApiGetBookInfoService> _logger;
    private TaaghchehSettings _taaghchehSettings;

    public WebApiGetBookInfoService(IHttpClientFactory httpClientFactory, IOptions<TaaghchehSettings> options, ILogger<WebApiGetBookInfoService> logger)
    {
        _taaghchehSettings = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(_taaghchehSettings.GetBookInfoApiEndpoint);
        _logger = logger;
    }

    public override async Task<BookInfo> GetBookInfoAsync(long bookId)
    {
        _logger.LogInformation($"Getting book info. Book id:{bookId}");

        BookInfo result = await _httpClient.GetFromJsonAsync<BookInfo>(bookId.ToString());

        _logger.LogInformation($"Is successfull? {result is not null}");

        return result;

        //if (_nextService is not null)
        //{
        //    // add code here in case next service(layer) is added in future

        //}
    }
}