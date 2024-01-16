namespace MyNewsWebApi.Infrastructure;

public class ApiSettings
{
    public string? Name { get; set; }
    public double CacheSlidingExpirationSeconds { get; set; }
}