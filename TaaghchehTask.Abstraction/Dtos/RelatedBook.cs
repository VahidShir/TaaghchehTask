namespace TaaghchehTask.Abstraction.Dtos;

public record RelatedBook
{
    public int Type { get; set; }
    public string Title { get; set; }
    public int BackgroundStyle { get; set; }
    public BookData BookData { get; set; }
    public Destination Destination { get; set; }
    public BackgroundConfig BackgroundConfig { get; set; }
}