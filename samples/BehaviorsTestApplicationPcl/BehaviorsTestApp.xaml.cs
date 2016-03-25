using Perspex;
using Perspex.Markup.Xaml;

namespace BehaviorsTestApplication
{
    public abstract class BehaviorsTestApp : Application
    {
        protected abstract void RegisterPlatform();

        public BehaviorsTestApp()
        {
            RegisterServices();
            RegisterPlatform();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var loader = new PerspexXamlLoader();
            loader.Load(typeof(BehaviorsTestApp), this);
        }
    }
}
