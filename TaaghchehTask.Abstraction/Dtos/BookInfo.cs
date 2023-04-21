using System.Text.Json;

namespace TaaghchehTask.Abstraction.Dtos;

public record BookInfo
{
    public Book Book { get; set; }
    public IList<BookComment> Comments { get; set; }
    public int CommentsCount { get; set; }
    public IList<RelatedBook> RelatedBooks { get; set; }
    public string ShareText { get; set; }
    public IList<object> Quotes { get; set; }
    public int QuotesCount { get; set; }
    public bool HideOffCoupon { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}