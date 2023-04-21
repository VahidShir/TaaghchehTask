using System.Text.Json;

using TaaghchehTask.Abstraction.Dtos;

namespace TaaghchehTask.Service;

internal static class Extensions
{
    public static BookInfo ToBookInfo(this string bookInfoInString)
    {
        return JsonSerializer.Deserialize<BookInfo>(bookInfoInString);
    }
}