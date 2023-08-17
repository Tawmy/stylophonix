using Stylophonix.Shared;

namespace Stylophonix.Data;

public record NavOption(string Text, string IconUrl, IEnumerable<PopupMenuOption>? Options,
    string? DestinationUrl = null)
{
    public NavOption(string Text, string IconUrl, string? DestinationUrl = null) : this(Text, IconUrl,
        new[] {new PopupMenuOption(Text, true)}, DestinationUrl)
    {
    }

    public PopupMenu? PopupMenu { get; set; }
}