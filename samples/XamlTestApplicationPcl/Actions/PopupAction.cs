using Perspex;
using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Metadata;
using Perspex.Xaml.Interactivity;

namespace XamlTestApplication.Actions
{
    public class PopupAction : PerspexObject, IAction
    {
        public static readonly PerspexProperty ChildProperty =
            PerspexProperty.Register<PopupAction, Control>(nameof(Child));

        [Content]
        public Control Child
        {
            get { return (Control)this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        private Popup _popup = null;

        public object Execute(object sender, object parameter)
        {
            if (_popup == null)
            {
                _popup = new Popup()
                {
                    PlacementMode = PlacementMode.Pointer,
                    PlacementTarget = sender as Control,
                    StaysOpen = false
                };
            }
            _popup.Child = Child;
            _popup.Open();
            return null;
        }
    }
}
