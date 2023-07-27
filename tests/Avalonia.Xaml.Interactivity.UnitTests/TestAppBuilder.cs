using Avalonia.Headless;
using Avalonia.Xaml.Interactivity.UnitTests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace Avalonia.Xaml.Interactivity.UnitTests;

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
