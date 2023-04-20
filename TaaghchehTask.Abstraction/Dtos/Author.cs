namespace TaaghchehTask.Abstraction.Dtos;

public record Author
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Type { get; set; }
    public string Slug { get; set; }
}