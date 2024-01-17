using MyNewsWebApi.Entities;

namespace MyNewsWebApi.Handlers;

public class StoryHttpClientHandler(HttpClient httpClient) : IHttpClientHandler<Story?, int>
{
    public Task<Story?> GetEntityById(int id)
    {
        return httpClient.GetFromJsonAsync<Story?>($"/v0/item/{id}.json");
    }

    public Task<IEnumerable<int>?> GetIds()
    {
        return httpClient.GetFromJsonAsync<IEnumerable<int>?>("/v0/beststories.json");
    }
}