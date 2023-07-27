using Avalonia.Headless;
using Avalonia.Xaml.Interactions.UnitTests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace Avalonia.Xaml.Interactions.UnitTests;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder
            .Configure<App>()
            .UseSkia()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false });
    }
}
