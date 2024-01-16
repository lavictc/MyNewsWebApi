using Microsoft.Extensions.Caching.Memory;

namespace MyNewsWebApi.Services.Factories;

public class MemoryCacheEntryOptionsFactory : IMemoryCacheEntryOptionsFactory
{
    public MemoryCacheEntryOptions Create(TimeSpan timeSpan)
    {
        return new MemoryCacheEntryOptions {SlidingExpiration = timeSpan};
    }
}