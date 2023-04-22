using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Service;

internal class FileGetBookInfoService : AbstractGetBookInfoServiceHandler, IFileGetBookInfoService
{
    public override async Task<BookInfo> GetBookInfoAsync(long bookInfo)
    {
        // Here we must write the code to save and retrive book info in file system.
        // Here we merely delegate this responsibility to next handler since it's just for demonstration
        return await _nextService.GetBookInfoAsync(bookInfo);
    }
}