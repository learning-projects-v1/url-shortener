using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UrlShortenerService _urlShortenerService;

    public UrlShortenerController(AppDbContext context, UrlShortenerService urlService)
    {
        _context = context;
        _urlShortenerService = urlService;
    }

    [HttpPost]
    public async Task<IActionResult> GetShortUrl([FromQuery] string originalUrl)
    {
        var shortUrl = await _urlShortenerService.CreateShortUrl(originalUrl);
        var request = HttpContext.Request;
        var baseAddress = $"{request.Scheme}://{request.Host.Value}";
        return Ok(new { Url = $"{baseAddress}{request.Path}/{shortUrl.ShortenedUrl}"});
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectUrl(string shortUrl)
    {
        var originalUrl = await _urlShortenerService.ResolveShortUrlAsync(shortUrl);
        if(originalUrl == null)
        {
            return NotFound("Url not found");
        }
        if (!originalUrl.StartsWith("http"))
        {
            originalUrl = $"https://{originalUrl}";
        }
        return Redirect(originalUrl);
    }

    [HttpGet("Metadata")]
    public IActionResult GetMetadata(string shortUrl)
    {
        return Ok();
    }
}
