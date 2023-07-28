using System.Windows.Input;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

public partial class InvokeCommandAction003 : Window
{
    public ICommand TestCommand { get; set; }

    public InvokeCommandAction003()
    {
        InitializeComponent();

        TestCommand = new Command(parameter =>
        {
            TargetTextBox.Text = $"Command {parameter}";
        });

        DataContext = this;
    }
}
