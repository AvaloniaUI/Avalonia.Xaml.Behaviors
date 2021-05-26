using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BehaviorsTestApplication.Pages
{
    public class ValueChangedTriggerBehaviorControl : UserControl
    {
        public ValueChangedTriggerBehaviorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
