namespace TaaghchehTask.Abstraction.Configs;

public class TaaghchehSettings
{
    public string GetBookInfoApiEndpoint { get; set; }
    public int InMemoryCachExpirationTimeInSeconds { get; set; }
    public string RedisAddress { get; set; }
}