namespace TaaghchehTask.Abstraction.Dtos;

public record Category
{
    public int Id { get; set; }
    public int Parent { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Icon { get; set; }
}