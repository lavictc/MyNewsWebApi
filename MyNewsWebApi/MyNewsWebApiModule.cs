using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Infrastructure.IoC;
using MyNewsWebApi.Services;

namespace MyNewsWebApi;

public class MyNewsWebApiModule : Module
{
    protected override void Load(IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient<IHttpClientHandler<Story?, int>, StoryHttpClientHandler>(client =>
        {
            client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/beststories.json");
        });
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton<IStoryService, NewsStoryService>();
    }
}