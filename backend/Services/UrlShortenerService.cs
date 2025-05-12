using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Data;
using UrlShortener.Models;
namespace UrlShortener.Services;

public class UrlShortenerService
{
    private readonly AppDbContext _appDbContext;
    IConfiguration _configuration;
    IDistributedCache _cache;
    private readonly int _maxLength;
    public UrlShortenerService(AppDbContext appDbContext, IDistributedCache cache, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;
        _cache = cache;
        _maxLength = _configuration.GetValue<int>("Configuration:MaxLength");
    }

    private async Task<ShortUrl> CreateUrl(string originalUrl)
    {
        const int maxTry = 5;
        for (int i = 0; i < maxTry; i++)
        {
            string url = GenerateRandomString(_maxLength);
            if (await _appDbContext.ShortUrls.FindAsync(url) == null)
            {
                var shortUrl = new ShortUrl
                {
                    ShortenedUrl = url,
                    OriginalUrl = originalUrl,
                    Clicks = 0,
                    CreatedAt = DateTime.UtcNow
                };
                _appDbContext.ShortUrls.Add(shortUrl);
                await _appDbContext.SaveChangesAsync();
                return shortUrl;
            }
        }
        throw new Exception("Could't not generate short url");
    }

    public string GenerateRandomString(int maxLength)
    {
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890";
        string url = "";
        var random = new Random();
        return new string(
            Enumerable.Range(0, maxLength)
            .Select(id => alphabets[random.Next(alphabets.Length)])
            .ToArray()
        );
    }

    public async Task<ShortUrl> CreateShortUrl(string originalUrl)
    {
        var key = $"originurl:{originalUrl}";
        var cachedUrl = await _cache.GetStringAsync(key);
        if (cachedUrl != null)
        {
            return new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = cachedUrl,
                CreatedAt = DateTime.UtcNow
            };
        }

        var shortUrl = await CreateUrl(originalUrl);
        await _cache.SetStringAsync($"shorturl:{shortUrl.ShortenedUrl}", shortUrl.OriginalUrl);
        await _cache.SetStringAsync(key, shortUrl.ShortenedUrl);
        return shortUrl;
    }

    public async Task<string> ResolveShortUrlAsync(string shortUrl)
    {
        var originalUrlCache = await _cache.GetStringAsync($"shorturl:{shortUrl}");
        if(originalUrlCache != null)
        {
            return originalUrlCache;
        }

        var shortUrlModel = await _appDbContext.ShortUrls.FindAsync(shortUrl);
        if (shortUrlModel == null) return null;
        await _cache.SetStringAsync($"shorturl:{shortUrl}", shortUrlModel.OriginalUrl);
        await _cache.SetStringAsync($"originalurl:{shortUrlModel.OriginalUrl}", shortUrl);

        return shortUrlModel.OriginalUrl;
    }
}
