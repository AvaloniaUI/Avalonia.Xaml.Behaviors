using System.Windows.Input;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public partial class InvokeCommandAction001 : Window
{
    public ICommand TestCommand { get; set; }

    public InvokeCommandAction001()
    {
        InitializeComponent();

        TestCommand = new Command(_ =>
        {
            TargetTextBox.Text = "Command Text";
        });

        DataContext = this;
    }
}
