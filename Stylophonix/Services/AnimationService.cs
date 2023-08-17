using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Stylophonix.Interfaces;

namespace Stylophonix.Services;

public class AnimationService : IAnimationService
{
    private readonly NavigationManager _navigationManager;
    private bool _locationChanged;

    public AnimationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += NavManagerOnLocationChanged;
    }

    public bool ShouldAnimate()
    {
        if (_locationChanged)
        {
            return false;
        }

        var relPath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
        return string.IsNullOrWhiteSpace(relPath);
    }

    private void NavManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _locationChanged = true;
    }
}