using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public class DataTriggerBehaviorTests
{
    [AvaloniaFact]
    public void DataTriggerBehavior_001()
    {
        var window = new DataTriggerBehavior001();

        window.Show();

        Assert.Equal("Less than or equal 50", window.TargetTextBlock.Text);
        Assert.Equal("0", window.TargetTextBox.Text);
        Assert.Equal(0d, window.TargetSlider.Value);

        window.TargetSlider.Focus();
        window.KeyPress(Key.Right, RawInputModifiers.None);
        window.KeyPress(Key.Right, RawInputModifiers.None);
        window.KeyPress(Key.Right, RawInputModifiers.None);

        Assert.Equal("More than 50", window.TargetTextBlock.Text);
        Assert.Equal("75", window.TargetTextBox.Text);
        Assert.Equal(75d, window.TargetSlider.Value);
    }
}
