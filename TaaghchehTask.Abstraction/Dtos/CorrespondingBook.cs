namespace TaaghchehTask.Abstraction.Dtos;

public record CorrespondingBook
{
    public string Title { get; set; }
    public string Color { get; set; }
    public string Icon { get; set; }
    public Destination Destination { get; set; }
}