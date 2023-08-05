using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Interactivity;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public class InvokeCommandActionTests
{
    [AvaloniaFact]
    public void InvokeCommandAction_001()
    {
        var window = new InvokeCommandAction001();

        window.Show();
        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_001_0.png");

        Assert.Equal("Initial Text", window.TargetTextBox.Text);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_001_1.png");

        Assert.Equal("Command Text", window.TargetTextBox.Text);
    }

    [AvaloniaFact]
    public void InvokeCommandAction_002()
    {
        var window = new InvokeCommandAction002();

        window.Show();
        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_002_0.png");

        Assert.Equal("Initial Text", window.TargetTextBox.Text);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_002_1.png");

        Assert.Equal("Command Param", window.TargetTextBox.Text);
    }

    [AvaloniaFact]
    public void InvokeCommandAction_003()
    {
        var window = new InvokeCommandAction003();

        window.Show();
        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_003_0.png");

        Assert.Equal("Initial Text", window.TargetTextBox.Text);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_003_1.png");

        Assert.Equal($"Command {nameof(Button)}", window.TargetTextBox.Text);
    }

    [AvaloniaFact]
    public void InvokeCommandAction_004()
    {
        var window = new InvokeCommandAction004();

        window.Show();
        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_004_0.png");

        Assert.Equal("Initial Text", window.TargetTextBox.Text);

        // Click
        window.TargetButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("InvokeCommandAction_004_1.png");

        Assert.Equal(nameof(RoutedEventArgs), window.TargetTextBox.Text);
    }
}
