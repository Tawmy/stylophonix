namespace Stylophonix.Data;

public record PopupMenuOption(
    string Title,
    bool Bold,
    string? Subtitle = null,
    string? Url = null,
    string? ThumbnailUrl = null);