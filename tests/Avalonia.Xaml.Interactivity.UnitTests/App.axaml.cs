using Avalonia.Markup.Xaml;

namespace Avalonia.Xaml.Interactivity.UnitTests;

using Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
