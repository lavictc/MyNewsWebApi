using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Infrastructure.IoC;
using MyNewsWebApi.Services;

namespace MyNewsWebApi.UnitTests;

public class MyNewsWebApiModuleTests
{
    private readonly WebApplication _app;

    public MyNewsWebApiModuleTests()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions());

        builder.Services.RegisterModule<MyNewsWebApiModule>();

        _app = builder.Build();
    }

    [Fact]
    public void StoryHttpClientHandler_resolves_Test()
    {
        var type = _app.Services.GetService<IHttpClientHandler<Story?, int>>();

        Assert.NotNull(type);
        Assert.Equal(typeof(StoryHttpClientHandler), type.GetType());
    }

    [Fact]
    public void IMapper_resolves_Test()
    {
        var type = _app.Services.GetService<IMapper>();

        Assert.NotNull(type);
        Assert.Equal(typeof(Mapper), type.GetType());
    }

    [Fact]
    public void IStoryService_resolves_Test()
    {
        var type = _app.Services.GetService<IStoryService>();

        Assert.NotNull(type);
        Assert.Equal(typeof(NewsStoryService), type.GetType());
    }
}