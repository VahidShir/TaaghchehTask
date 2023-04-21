namespace TaaghchehTask.Abstraction.Services;

/// <summary>
/// An inteface for implementing Chain of Responsibility design pattern used in different sources and caches
/// </summary>
public interface IGetBookInfoServiceHandler : IGetBookInfoService
{
    IGetBookInfoService SetNextLayer(IGetBookInfoService service);
}