using System.Text.Json;

using TaaghchehTask.Abstraction.Dtos;

using File = System.IO.File;

namespace TaaghchehTask.Test;

internal static class Helper
{
    public static async Task<BookInfo> GetSampleBookInfo()
    {
        var file = await File.ReadAllTextAsync("sampleBook.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        BookInfo bookInfo = JsonSerializer.Deserialize<BookInfo>(file, options);

        return bookInfo;
    }
}