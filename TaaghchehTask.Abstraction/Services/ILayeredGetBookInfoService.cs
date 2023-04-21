namespace TaaghchehTask.Abstraction.Services;

/// <summary>
/// A service for getting book info with multiple layers of caches using Chain of Responsibility design pattern
/// </summary>
public interface ILayeredGetBookInfoService : IGetBookInfoService
{
}