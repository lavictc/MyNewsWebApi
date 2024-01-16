using MyNewsWebApi.Handlers;

namespace MyNewsWebApi.IntegrationTests.Handlers;

public class StoryHttpClientHandlerTests
{
    [Fact]
    public async void GetIds_is_not_empty_Test()
    {
        var handler = new StoryHttpClientHandler(new HttpClient { BaseAddress = new Uri("https://hacker-news.firebaseio.com") });

        var ids = await handler.GetIds();

        Assert.NotNull(ids);
        Assert.NotEmpty(ids);
    }

    [Fact]
    public async void GetEntityById_is_not_null_Test()
    {
        var handler = new StoryHttpClientHandler(new HttpClient { BaseAddress = new Uri("https://hacker-news.firebaseio.com") });

        var ids = await handler.GetIds();

        Assert.NotNull(ids);

        var entity = await handler.GetEntityById(ids.Last());

        Assert.NotNull(entity);
    }
}