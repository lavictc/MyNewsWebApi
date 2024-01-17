namespace MyNewsWebApi.Entities;

public class Story
{
    public string? Title { get; init; } 
    public string? Url { get; init; }
    public string? By { get; init; }
    public long Time { get; init; }
    public int  Score { get; init; }
    public int CommentCount { get; init; }
}