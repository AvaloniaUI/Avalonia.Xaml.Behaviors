using System.Windows.Input;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public partial class InvokeCommandAction004 : Window
{
    public ICommand TestCommand { get; set; }

    public InvokeCommandAction004()
    {
        InitializeComponent();

        TestCommand = new Command(parameter =>
        {
            TargetTextBox.Text = $"{parameter?.GetType().Name}";
        });

        DataContext = this;
    }
}
