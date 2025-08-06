namespace Stylophonix.Data;

public record GigInfo
{
    public required string Id { get; init; }
    public required string NavTitle { get; init; }
    public required string NavSubtitle { get; init; }
    public required string Url { get; init; }
    public required int NavOrder { get; init; }

    public required string Title { get; init; }
    public required IEnumerable<string> Paragraphs { get; init; }

    public required IEnumerable<string> ImageUrls { get; init; }
    public required IEnumerable<string> VideoUrls { get; init; }
}