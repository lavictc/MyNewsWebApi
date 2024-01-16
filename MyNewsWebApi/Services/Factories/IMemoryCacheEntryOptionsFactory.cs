using Microsoft.Extensions.Caching.Memory;

namespace MyNewsWebApi.Services.Factories;

public interface IMemoryCacheEntryOptionsFactory
{
    MemoryCacheEntryOptions Create(TimeSpan timeSpan);
}