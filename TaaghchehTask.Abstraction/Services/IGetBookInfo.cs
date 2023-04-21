using TaaghchehTask.Abstraction.Dtos;

namespace TaaghchehTask.Abstraction.Services;

public interface IGetBookInfoService
{
    Task<BookInfo> GetBookInfoAsync(long bookId);
}