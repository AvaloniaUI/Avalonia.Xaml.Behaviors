using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public class ChangePropertyActionTests
{
    /// <summary>
    /// Regular property.
    /// </summary>
    [AvaloniaFact]
    public void ChangePropertyAction_001()
    {
        var window = new ChangePropertyAction001();

        window.Show();
        window.CaptureRenderedFrame()?.Save("ChangePropertyAction_001_0.png");

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("ChangePropertyAction_001_1.png");

        Assert.Equal("Updated Text", window.TargetTextBox.Text);
    }

    /// <summary>
    /// Attached property.
    /// </summary>
    [AvaloniaFact]
    public void ChangePropertyAction_002()
    {
        var window = new ChangePropertyAction002();

        window.Show();
        window.CaptureRenderedFrame()?.Save("ChangePropertyAction_002_0.png");

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("ChangePropertyAction_002_1.png");

        Assert.Equal(12d, window.TargetTextBox.FontSize);
    }
}
