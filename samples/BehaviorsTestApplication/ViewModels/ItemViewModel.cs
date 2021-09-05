using BehaviorsTestApplication.ViewModels.Core;

namespace BehaviorsTestApplication.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => Update(ref _name, value);
        }

        public ItemViewModel(string name)
        {
            _name = name;
        }

        public override string ToString() => _name;
    }
}
