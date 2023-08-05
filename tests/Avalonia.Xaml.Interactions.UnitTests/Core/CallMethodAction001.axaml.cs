using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public partial class CallMethodAction001 : Window
{
    public string? TestProperty { get; set; }
    
    public CallMethodAction001()
    {
        InitializeComponent();
    }

    public void TestMethod()
    {
        TestProperty = "Test String";
    }
}
