using Microsoft.AspNetCore.Mvc;

using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBookInfoAsync(long bookId, [FromServices] IWebApiGetBookInfoService webApiGetBookInfoService)
    {
        BookInfo bookInfo = await webApiGetBookInfoService.GetBookInfoAsync(bookId);

        return Ok(bookInfo);
    }
}