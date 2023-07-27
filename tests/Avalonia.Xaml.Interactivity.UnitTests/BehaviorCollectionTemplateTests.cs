using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Xunit;

namespace Avalonia.Xaml.Interactivity.UnitTests;

public class BehaviorCollectionTemplateTests
{
    [AvaloniaFact]
    public void BehaviorCollectionTemplate_001()
    {
        var window = new BehaviorCollectionTemplate001();

        window.Show();

        var containers = window.TargetListBox.GetRealizedContainers().Cast<ListBoxItem>().ToList();

        var behaviors0 = containers[0].GetValue(Interaction.BehaviorsProperty);
        Assert.NotNull(behaviors0);
        Assert.Single(behaviors0);
        Assert.Equal(containers[0], behaviors0!.AssociatedObject);

        var behaviors1 = containers[1].GetValue(Interaction.BehaviorsProperty);
        Assert.NotNull(behaviors1);
        Assert.Single(behaviors1);
        Assert.Equal(containers[1], behaviors1!.AssociatedObject);

        var behaviors2 = containers[2].GetValue(Interaction.BehaviorsProperty);
        Assert.NotNull(behaviors2);
        Assert.Single(behaviors2);
        Assert.Equal(containers[2], behaviors2!.AssociatedObject);

        Assert.Equal(containers[0].Background, Brushes.Transparent);
        Assert.Equal(containers[1].Background, Brushes.Transparent);
        Assert.Equal(containers[2].Background, Brushes.Transparent);

        // KeyDown
        containers[0].Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal(window.Resources["RedBrush"], containers[0].Background);

        // KeyDown
        containers[1].Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal(window.Resources["RedBrush"], containers[1].Background);

        // KeyDown
        containers[2].Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal(window.Resources["RedBrush"], containers[2].Background);
    }
}
