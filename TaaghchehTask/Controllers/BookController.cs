using Microsoft.AspNetCore.Mvc;

namespace TaaghchehTask.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBookInfoAsync(long bookId)
    {
        return Ok();
    }
}