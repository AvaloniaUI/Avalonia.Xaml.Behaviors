using Avalonia.Markup.Xaml;

namespace Avalonia.Xaml.Interactions.UnitTests;

using Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
