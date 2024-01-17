using System.Text.Json.Serialization;

namespace MyNewsWebApi.Entities;

public class Story
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("by")]
    public string? By { get; init; }

    [JsonPropertyName("time")]
    public long Time { get; init; }

    [JsonPropertyName("score")]
    public int  Score { get; init; }

    [JsonPropertyName("kids")]
    public int[]?  Kids { get; init; }
}