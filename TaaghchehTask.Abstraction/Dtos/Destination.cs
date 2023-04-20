namespace TaaghchehTask.Abstraction.Dtos;

public record Destination
{
    public int Type { get; set; }
    public int Order { get; set; }
    public int NavigationPage { get; set; }
    public int OperationType { get; set; }
    public int BookId { get; set; }
}