namespace TaaghchehTask.Abstraction.Dtos;

public record BookData
{
    public IList<Book> Books { get; set; }
    public int Layout { get; set; }
    public bool ShowPrice { get; set; }
}