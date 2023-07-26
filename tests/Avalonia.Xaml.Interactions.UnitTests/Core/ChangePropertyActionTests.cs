using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;
using Xunit;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public class ChangePropertyActionTests
{
    [AvaloniaFact]
    public void Should_Update_Text_Property_On_Click_Event()
    {
        var textBox = new TextBox
        {
            Text = "Initial Text"
        };

        var button = new Button
        {
            [Interaction.BehaviorsProperty] = new BehaviorCollection 
            {
                new EventTriggerBehavior
                {
                    EventName = "Click",
                    Actions =
                    {
                        new ChangePropertyAction 
                        { 
                            TargetObject = textBox,
                            PropertyName = "Text",
                            Value = "Updated Text"
                        }
                    }
                }
            }
        };

        var window = new Window
        {
            Content = new StackPanel
            {
                Children =
                {
                    textBox,
                    button
                }
            }
        };

        window.Show();

        // Click
        button.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal("Updated Text", textBox.Text);
    }
}
