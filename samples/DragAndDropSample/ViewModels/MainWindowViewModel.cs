using System.Collections.ObjectModel;
using ReactiveUI;

namespace DragAndDropSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _items;
        
        public ObservableCollection<ItemViewModel> Items
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }

        public MainWindowViewModel()
        {
            _items = new ObservableCollection<ItemViewModel>()
            {
                new() { Title = "Item0" },
                new() { Title = "Item1" },
                new() { Title = "Item2" },
                new() { Title = "Item3" },
                new() { Title = "Item4" }
            };
        }
    }
}
