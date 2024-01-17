namespace MyNewsWebApi.Infrastructure;

public class ApiSettings
{
    public string? Name { get; init; }
    public double CacheSlidingExpirationSeconds { get; init; }
}