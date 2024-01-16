using AutoMapper;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Models;

namespace MyNewsWebApi.Services;

public class NewsStoryService(
    IHttpClientHandler<Story?, int> storyHttpClientHandler,
    IMapper mapper) : IStoryService
    //todo: inject mem cache impl
{
    public async Task<IEnumerable<StoryDto>> GetTheBestStories(int n)
    {
        var storyIds = await storyHttpClientHandler.GetIds();
        var stories = new List<StoryDto>();

        var tasks = storyIds?.Select(async id =>
        {
            //todo: check the cache
            var story = await storyHttpClientHandler.GetEntityById(id);

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