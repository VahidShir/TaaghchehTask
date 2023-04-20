namespace TaaghchehTask.Abstraction.Dtos;

public record Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int SourceBookId { get; set; }
    public bool HasPhysicalEdition { get; set; }
    public int CanonicalId { get; set; }
    public string Description { get; set; }
    public string HtmlDescription { get; set; }
    public int PublisherID { get; set; }
    public string PublisherSlug { get; set; }
    public double Price { get; set; }
    public int NumberOfPages { get; set; }
    public double Rating { get; set; }
    public IList<Rate> Rates { get; set; }
    public IList<RateDetail> RateDetails { get; set; }
    public int BeforeOffPrice { get; set; }
    public bool IsRtl { get; set; }
    public bool ShowOverlay { get; set; }
    public int PhysicalPrice { get; set; }
    public int PhysicalBeforOffPrice { get; set; }
    public string ISBN { get; set; }
    public string PublishDate { get; set; }
    public int Destination { get; set; }
    public string Type { get; set; }
    public string RefId { get; set; }
    public string CoverUri { get; set; }
    public string ShareUri { get; set; }
    public string ShareText { get; set; }
    public string Publisher { get; set; }
    public IList<Author> Authors { get; set; }
    public IList<File> Files { get; set; }
    public IList<Label> Labels { get; set; }
    public IList<Category> Categories { get; set; }
    public bool SubscriptionAvailable { get; set; }
    public int State { get; set; }
    public bool Encrypted { get; set; }
    public double CurrencyPrice { get; set; }
    public double CurrencyBeforeOffPrice { get; set; }
    public string Sticker { get; set; }
    public string OffText { get; set; }
    public string PriceColor { get; set; }
    public string Subtitle { get; set; }
    public IList<CorrespondingBook> CorrespondingBooks { get; set; }
    public string JsonDescription { get; set; }
    public string Faqs { get; set; }
    public IList<Review> Reviews { get; set; }
}