using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Infrastructure;
using MyNewsWebApi.Models;
using MyNewsWebApi.Services;
using MyNewsWebApi.Services.Factories;

namespace MyNewsWebApi.UnitTests.Services;

public class NewsStoryServiceTests
{
    private readonly Mock<IHttpClientHandler<Story?, int>> _httpClientHandler;
    private readonly Mock<IMapper> _mapper;
    private readonly NewsStoryService _service;
    private readonly Mock<IMemoryCacheEntryOptionsFactory> _factory;
    private readonly Mock<IOptions<ApiSettings>> _apiSettings;

    public NewsStoryServiceTests()
    {
        _httpClientHandler = new Mock<IHttpClientHandler<Story?, int>>(MockBehavior.Strict);
        _mapper = new Mock<IMapper>(MockBehavior.Strict);
        _factory = new Mock<IMemoryCacheEntryOptionsFactory>(MockBehavior.Strict);
        _apiSettings = new Mock<IOptions<ApiSettings>>(MockBehavior.Strict);

        var memoryCache = new MemoryCache(new MemoryCacheOptions()); //shame we're unable to mock an IMemoryCache. Can't mock method extensions (IMemoryCache.TryGetValue,  IMemoryCache.Set etc.).

        _service = new NewsStoryService(_httpClientHandler.Object, _mapper.Object, memoryCache, _factory.Object, _apiSettings.Object);
    }

    [Fact]
    public async void Request_for_story_ids_returns_null_Test()
    {
        IEnumerable<int>? storyIds = null;

        _httpClientHandler.Setup(x => x.GetIds()).ReturnsAsync(storyIds);

        var result = await _service.GetTheBestStories(10);

        Assert.Empty(result);

        _httpClientHandler.VerifyAll();
        _mapper.VerifyAll();
        _factory.VerifyAll();
        _apiSettings.VerifyAll();
    }

    [Fact]
    public async void Request_for_a_story_returns_null_Test()
    {
        const int storyId = 1;
        Story story = null!;
        const int cacheSlidingExpirationSeconds = 5;
        var apiSettings = new ApiSettings { CacheSlidingExpirationSeconds = cacheSlidingExpirationSeconds };
        var timeSpan = TimeSpan.FromMicroseconds(cacheSlidingExpirationSeconds);
        var cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = timeSpan };

        IEnumerable<int> storyIds = new List<int> {storyId};

        _httpClientHandler.Setup(x => x.GetIds()).ReturnsAsync(storyIds);
        _httpClientHandler.Setup(x => x.GetEntityById(storyId)).ReturnsAsync(story);

        _factory.Setup(x => x.Create(timeSpan)).Returns(cacheEntryOptions);

        _apiSettings.Setup(x => x.Value).Returns(apiSettings);

        var result = await _service.GetTheBestStories(10);

        Assert.Empty(result);

        _httpClientHandler.VerifyAll();
        _mapper.VerifyAll();
        _factory.VerifyAll();
        _apiSettings.VerifyAll();
    }

    [Fact]
    public async void Request_for_a_story_returns_a_story_dto_Test()
    {
        const int storyId = 1;
        var story = new Story();
        var storyDto = new StoryDto();
        const int cacheSlidingExpirationSeconds = 5;
        var timeSpan = TimeSpan.FromMicroseconds(cacheSlidingExpirationSeconds);
        var cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = timeSpan };
        var apiSettings = new ApiSettings{CacheSlidingExpirationSeconds = cacheSlidingExpirationSeconds};

        IEnumerable<int> storyIds = new List<int> { storyId };

        _httpClientHandler.Setup(x => x.GetIds()).ReturnsAsync(storyIds);
        _httpClientHandler.Setup(x => x.GetEntityById(storyId)).ReturnsAsync(story);

        _mapper.Setup(x => x.Map<Story, StoryDto>(story)).Returns(storyDto);

        _factory.Setup(x => x.Create(timeSpan)).Returns(cacheEntryOptions);

        _apiSettings.Setup(x => x.Value).Returns(apiSettings);

        var result = await _service.GetTheBestStories(10);

        Assert.Equal(storyIds.Count(), result.Count());

        _httpClientHandler.VerifyAll();
        _mapper.VerifyAll();
        _factory.VerifyAll();
        _apiSettings.VerifyAll();
    }

    [Fact]
    public async void Stories_are_ordered_descending_by_score_Test()
    {
        const int cacheSlidingExpirationSeconds = 5;
        var apiSettings = new ApiSettings { CacheSlidingExpirationSeconds = cacheSlidingExpirationSeconds };
        var timeSpan = TimeSpan.FromMicroseconds(cacheSlidingExpirationSeconds);
        var cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = timeSpan };

        const int storyId1 = 1;
        const int storyId2 = 2;
        const int storyId3 = 3;
        const int storyId4 = 4;
        const int storyId5 = 5;

        var story1 = new Story();
        var story2 = new Story();
        var story3 = new Story();
        var story4 = new Story();
        var story5 = new Story();

        var storyDto1 = new StoryDto{ Score = 1 };
        var storyDto2 = new StoryDto{ Score = 2 };
        var storyDto3 = new StoryDto{ Score = 3 };
        var storyDto4 = new StoryDto{ Score = 4 };
        var storyDto5 = new StoryDto{ Score = 5 };

        IEnumerable<int> storyIds = new List<int> { storyId1, storyId5, storyId3, storyId2, storyId4 };

        _httpClientHandler.Setup(x => x.GetIds()).ReturnsAsync(storyIds);

        var sequence = new MockSequence();

        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId1)).ReturnsAsync(story1);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId5)).ReturnsAsync(story5);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId3)).ReturnsAsync(story3);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId2)).ReturnsAsync(story2);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId4)).ReturnsAsync(story4);

        sequence = new MockSequence();

        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story1)).Returns(storyDto1);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story5)).Returns(storyDto5);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story3)).Returns(storyDto3);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story2)).Returns(storyDto2);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story4)).Returns(storyDto4);

        _apiSettings.Setup(x => x.Value).Returns(apiSettings);

        _factory.Setup(x => x.Create(timeSpan)).Returns(cacheEntryOptions);

        var result = await _service.GetTheBestStories(5);

        var descScores = string.Join(",", result.Select(x => x.Score));

        Assert.Equal("5,4,3,2,1", descScores);

        _httpClientHandler.VerifyAll();
        _mapper.VerifyAll();
        _factory.VerifyAll();
        _apiSettings.VerifyAll();
    }

    [Fact]
    public async void Take_top_three_stories_descending_by_score_Test()
    {
        const int cacheSlidingExpirationSeconds = 5;
        var apiSettings = new ApiSettings { CacheSlidingExpirationSeconds = cacheSlidingExpirationSeconds };
        var timeSpan = TimeSpan.FromMicroseconds(cacheSlidingExpirationSeconds);
        var cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = timeSpan };

        const int storyId1 = 1;
        const int storyId2 = 2;
        const int storyId3 = 3;
        const int storyId4 = 4;
        const int storyId5 = 5;

        var story1 = new Story();
        var story2 = new Story();
        var story3 = new Story();
        var story4 = new Story();
        var story5 = new Story();

        var storyDto1 = new StoryDto { Score = 1 };
        var storyDto2 = new StoryDto { Score = 2 };
        var storyDto3 = new StoryDto { Score = 3 };
        var storyDto4 = new StoryDto { Score = 4 };
        var storyDto5 = new StoryDto { Score = 5 };

        IEnumerable<int> storyIds = new List<int> { storyId1, storyId5, storyId3, storyId2, storyId4 };

        _httpClientHandler.Setup(x => x.GetIds()).ReturnsAsync(storyIds);

        var sequence = new MockSequence();

        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId1)).ReturnsAsync(story1);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId5)).ReturnsAsync(story5);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId3)).ReturnsAsync(story3);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId2)).ReturnsAsync(story2);
        _httpClientHandler.InSequence(sequence).Setup(x => x.GetEntityById(storyId4)).ReturnsAsync(story4);

        sequence = new MockSequence();

        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story1)).Returns(storyDto1);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story5)).Returns(storyDto5);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story3)).Returns(storyDto3);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story2)).Returns(storyDto2);
        _mapper.InSequence(sequence).Setup(x => x.Map<Story, StoryDto>(story4)).Returns(storyDto4);

        _apiSettings.Setup(x => x.Value).Returns(apiSettings);

        _factory.Setup(x => x.Create(timeSpan)).Returns(cacheEntryOptions);

        var result = await _service.GetTheBestStories(3);

        var descScores = string.Join(",", result.Select(x => x.Score));

        Assert.Equal("5,4,3", descScores);

        _httpClientHandler.VerifyAll();
        _mapper.VerifyAll();
        _factory.VerifyAll();
        _apiSettings.VerifyAll();
    }

    //todo:test caching
}