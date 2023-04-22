using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class LayeredGetBookInfoService : ILayeredGetBookInfoService
{
    private readonly IEnumerable<IGetBookInfoServiceHandler> _handlers;
    private IGetBookInfoServiceHandler _firstHandler;

    public LayeredGetBookInfoService(IEnumerable<IGetBookInfoServiceHandler> handlers)
    {
        _handlers = handlers;

        SetLayers();
    }

    public async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        return await _firstHandler.GetBookInfoAsync(bookInfo);
    }

    private void SetLayers()
    {
        var inMemoryCacheHandler = _handlers.Single(h => h.GetType() == typeof(InMemoryCacheGetBookInfoService));
        var fileCacheHandler = _handlers.Single(h => h.GetType() == typeof(FileGetBookInfoService));
        var webApiHandler = _handlers.Single(h => h.GetType() == typeof(WebApiGetBookInfoService));

        inMemoryCacheHandler.SetNextLayer(fileCacheHandler);
        fileCacheHandler.SetNextLayer(webApiHandler);

        _firstHandler =  inMemoryCacheHandler;
    }
}