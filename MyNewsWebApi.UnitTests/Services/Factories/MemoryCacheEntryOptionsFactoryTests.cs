using MyNewsWebApi.Services.Factories;

namespace MyNewsWebApi.UnitTests.Services.Factories;

public class MemoryCacheEntryOptionsFactoryTests
{
    private readonly MemoryCacheEntryOptionsFactory _factory = new();

    [Fact]
    public void Create_set_sliding_expiration_Test()
    {
        var timeSpan = TimeSpan.FromMicroseconds(60);

        var options = _factory.Create(timeSpan);

        Assert.Equal(timeSpan, options.SlidingExpiration);
    }
}