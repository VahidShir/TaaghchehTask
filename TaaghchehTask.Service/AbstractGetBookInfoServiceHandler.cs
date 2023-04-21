using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal abstract class AbstractGetBookInfoServiceHandler : IGetBookInfoServiceHandler
{
    protected IGetBookInfoService _nextService;

    public abstract Task<BookInfo> GetBookInfoAsync(long bookInfo);

    public IGetBookInfoService SetNextLayer(IGetBookInfoService service)
    {
        _nextService = service;
        return service;
    }
}