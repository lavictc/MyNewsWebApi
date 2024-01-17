using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Infrastructure;
using MyNewsWebApi.Models;
using MyNewsWebApi.Services.Factories;

namespace MyNewsWebApi.Services;

public class NewsStoryService(
    IHttpClientHandler<Story?, int> storyHttpClientHandler,
    IMapper mapper,
    IMemoryCache cache,
    IMemoryCacheEntryOptionsFactory cacheEntryOptionsFactory,
    IOptions<ApiSettings> apiSettings) : IStoryService
{
    public async Task<IEnumerable<StoryDto>> GetTheBestStories(int n)
    {
        var storyIds = await storyHttpClientHandler.GetIds();
        var stories = new List<StoryDto>();

        var tasks = storyIds?.Select(async id =>
        {
            if (!cache.TryGetValue(id, out Story? story))
            {
                story = await storyHttpClientHandler.GetEntityById(id);

                var cacheEntryOptions = cacheEntryOptionsFactory.Create(TimeSpan.FromSeconds(apiSettings.Value.CacheSlidingExpirationSeconds));

                cache.Set(id, story, cacheEntryOptions);
            }

            if (story != null)
            {
                var dto = mapper.Map<Story, StoryDto>(story);
                stories.Add(dto);
            }
        });

        if (tasks != null) Task.WaitAll(tasks.ToArray());

        return stories.OrderByDescending(s => s.Score).Take(n);
    }
}