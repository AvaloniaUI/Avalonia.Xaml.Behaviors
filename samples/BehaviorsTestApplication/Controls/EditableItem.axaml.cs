using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace BehaviorsTestApplication.Controls;

public partial class EditableItem : UserControl
{
    public static readonly StyledProperty<string?> TextProperty =
        TextBlock.TextProperty.AddOwner<EditableItem>(new(
            defaultBindingMode: BindingMode.TwoWay));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public EditableItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
