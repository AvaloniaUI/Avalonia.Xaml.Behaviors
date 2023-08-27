using System.Threading.Tasks;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using VerifyXunit;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

[UsesVerify]
public class DataTriggerBehaviorTests
{
    [AvaloniaFact]
    public Task DataTriggerBehavior_001()
    {
        var window = new DataTriggerBehavior001();

        window.Show();
        window.CaptureRenderedFrame()?.Save("DataTriggerBehavior_001_0.png");

        Assert.Equal("Less than or equal 50", window.TargetTextBlock.Text);
        Assert.Equal("0", window.TargetTextBox.Text);
        Assert.Equal(0d, window.TargetSlider.Value);

        window.TargetSlider.Focus();
        window.KeyPress(Key.Right, RawInputModifiers.None);
        window.KeyPress(Key.Right, RawInputModifiers.None);
        window.KeyPress(Key.Right, RawInputModifiers.None);

        window.CaptureRenderedFrame()?.Save("DataTriggerBehavior_001_1.png");

        Assert.Equal("More than 50", window.TargetTextBlock.Text);
        Assert.Equal("75", window.TargetTextBox.Text);
        Assert.Equal(75d, window.TargetSlider.Value);
        return Verifier.Verify(window);
    }
}
