using System.Threading.Tasks;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

[UsesVerify]
public class CallMethodActionTests
{
    /// <summary>
    /// Without parameters.
    /// </summary>
    [AvaloniaFact]
    public Task CallMethodAction_001()
    {
        var window = new CallMethodAction001();

        window.Show();
        window.CaptureRenderedFrame()?.Save("CallMethodAction_001_0.png");

        Assert.Null(window.TestProperty);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("CallMethodAction_001_1.png");

        Assert.Equal("Test String", window.TestProperty);
        return Verifier.Verify(window);
    }

    /// <summary>
    /// With event handler parameters.
    /// </summary>
    [AvaloniaFact]
    public Task CallMethodAction_002()
    {
        var window = new CallMethodAction002();

        window.Show();
        window.CaptureRenderedFrame()?.Save("CallMethodAction_002_0.png");

        Assert.Null(window.TestProperty);
        Assert.Null(window.Sender);
        Assert.Null(window.Args);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("CallMethodAction_002_1.png");

        Assert.Equal("Test String", window.TestProperty);
        Assert.Equal(window, window.Sender);
        Assert.IsType<RoutedEventArgs>(window.Args);
        return Verifier.Verify(window);
    }
}
