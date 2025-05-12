using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class ShortUrl
{
    public string OriginalUrl { get; set; } = string.Empty;
    [Key]
    public string ShortenedUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Clicks { get; set; } = 0;
}
