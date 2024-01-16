using MyNewsWebApi.Models;

namespace MyNewsWebApi.Services;

public interface IStoryService
{
    Task<IEnumerable<StoryDto>> GetTheBestStories(int n);
}