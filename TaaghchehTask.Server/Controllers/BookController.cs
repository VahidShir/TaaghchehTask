using Microsoft.AspNetCore.Mvc;

using System.Net;

using TaaghchehTask.Abstraction.Dtos;
using TaaghchehTask.Abstraction.Services;

namespace TaaghchehTask.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookInfoAsync(long bookId, [FromServices] ILayeredGetBookInfoService service)
    {
        _logger.LogInformation($"Getting book info. Book id:{bookId}");

        try
        {
            BookInfo bookInfo = await service.GetBookInfoAsync(bookId);

            if (bookInfo is not null)
            {
                _logger.LogInformation($"Successfully got book info.");

                return Ok(bookInfo);
            }
            else
            {
                _logger.LogWarning($"Could not get book info.");

                return NotFound();
            }


        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occured.");

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}