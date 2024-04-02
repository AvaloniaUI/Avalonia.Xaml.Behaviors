using System.Threading.Tasks;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using VerifyXunit;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

[UsesVerify]
public class EventTriggerBehaviorTests
{
    [AvaloniaFact]
    public Task EventTriggerBehavior_001()
    {
        var window = new EventTriggerBehavior001();

        window.Show();
        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_001_0.png");

        window.Click(window.TargetButton);

        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_001_1.png");

        Assert.Equal("Click Text", window.TargetTextBox.Text);
        return Verifier.Verify(window);
    }

    [AvaloniaFact]
    public Task EventTriggerBehavior_002()
    {
        var window = new EventTriggerBehavior002();

        window.Show();
        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_002_0.png");

        window.Click(window.TargetButton);

        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_002_1.png");

        Assert.Equal("Tapped Text", window.TargetTextBox.Text);
        return Verifier.Verify(window);
    }

    [AvaloniaFact]
    public Task EventTriggerBehavior_003()
    {
        var window = new EventTriggerBehavior003();

        window.Show();
        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_003_0.png");

        window.Click(window.TargetButton);
        window.Click(window.TargetButton);

        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_003_1.png");

        Assert.Equal("DoubleTapped Text", window.TargetTextBox.Text);
        return Verifier.Verify(window);
    }

    [AvaloniaFact]
    public Task EventTriggerBehavior_004()
    {
        var window = new EventTriggerBehavior004();

        window.Show();
        window.CaptureRenderedFrame()?.Save("EventTriggerBehavior_004_0.png");

        Assert.Equal("Loaded Text", window.TargetTextBox.Text);
        return Verifier.Verify(window);
    }
}
