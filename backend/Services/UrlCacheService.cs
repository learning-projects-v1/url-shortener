using Microsoft.Extensions.Caching.Distributed;

namespace UrlShortener.Services;

public class UrlCacheService
{
    public IDistributedCache _cache;
    public UrlCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

}
