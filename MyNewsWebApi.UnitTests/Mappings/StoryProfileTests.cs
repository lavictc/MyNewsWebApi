using AutoMapper;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Mappings;
using MyNewsWebApi.Models;

namespace MyNewsWebApi.UnitTests.Mappings;

public class StoryProfileTests
{
    private readonly IMapper? _mapper;

    public StoryProfileTests()
    {
        var configuration = new MapperConfiguration(mc => { mc.AddProfile(new StoryProfile()); });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Null_Entity_returns_null_Dto_Test()
    {
        Story entity = null!;

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.Null(dto);
    }

    [Fact]
    public void Entity_Descendants_is_equal_to_Dto_CommentCount_Test()
    {
        var entity = new Story{ Descendants = 12 };

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.CommentCount);
        Assert.Equal(entity.Descendants, dto.CommentCount);
    }

    [Fact]
    public void Entity_By_is_equal_to_Dto_PostedBy_Test()
    {
        var entity = new Story { By = "carlos l" };

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.PostedBy);
        Assert.Equal(entity.By, dto.PostedBy);
    }

    [Fact]
    public void Entity_Score_is_equal_to_Dto_Score_Test()
    {
        var entity = new Story { Score = 99 };

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.Score);
        Assert.Equal(entity.Score, dto.Score);
    }

    [Fact]
    public void Entity_Time_is_equal_to_Dto_Time_Test()
    {
        var entity = new Story { Time = 1705324926 };

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.Time);
        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(entity.Time).LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), dto.Time);
    }

    [Fact]
    public void Entity_Title_is_equal_to_Dto_Title_Test()
    {
        var entity = new Story { Title = "xyz" };

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.Title);
        Assert.Equal(entity.Title, dto.Title);
    }

    [Fact]
    public void Entity_Url_is_equal_to_Dto_Url_Test()
    {
        var entity = new Story {Url = "http:blah/"};

        var dto = _mapper?.Map<Story, StoryDto>(entity);

        Assert.NotNull(dto?.Url);
        Assert.Equal(entity.Url, dto.Url);
    }
}