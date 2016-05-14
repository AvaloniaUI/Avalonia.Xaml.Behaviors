using Avalonia;
using Avalonia.Markup.Xaml;

namespace BehaviorsTestApplication
{
    public class BehaviorsTestApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
