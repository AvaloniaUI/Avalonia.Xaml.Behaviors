using Perspex;
using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Media;
using Perspex.Xaml.Interactivity;

namespace XamlTestApplication.Actions
{
    public class PopupAction : PerspexObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            new Popup()
            {
                PlacementMode = PlacementMode.Pointer,
                PlacementTarget = sender as Control,
                StaysOpen = false,
                Child = new TextBlock
                {
                    Text = "Hello from custom action!",
                    Background = Brushes.Gray
                }
            }.Open();
            return null;
        }
    }
}
