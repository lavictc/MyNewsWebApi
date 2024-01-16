using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Handlers;
using MyNewsWebApi.Infrastructure;
using MyNewsWebApi.Infrastructure.IoC;
using MyNewsWebApi.Services;
using MyNewsWebApi.Services.Factories;

namespace MyNewsWebApi.UnitTests;

public class MyNewsWebApiModuleTests
{
    private readonly WebApplication _app;

    public MyNewsWebApiModuleTests()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions());

        builder.Services.RegisterModule<MyNewsWebApiModule>();
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

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

    [Fact]
    public void IMemoryCache_resolves_Test()
    {
        var type = _app.Services.GetService<IMemoryCache>();

        Assert.NotNull(type);
        Assert.Equal(typeof(MemoryCache), type.GetType());
    }

    [Fact]
    public void IMemoryCacheEntryOptionsFactory_resolves_Test()
    {
        var type = _app.Services.GetService<IMemoryCacheEntryOptionsFactory>();

        Assert.NotNull(type);
        Assert.Equal(typeof(MemoryCacheEntryOptionsFactory), type.GetType());
    }

    [Fact]
    public void ApiSettings_resolves_Test()
    {
        var type = _app.Services.GetService<IOptions<ApiSettings>>();

        Assert.NotNull(type);
        Assert.Equal(typeof(ApiSettings), type.Value.GetType());
    }
}