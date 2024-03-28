using System.Collections.ObjectModel;
using BehaviorsTestApplication.ViewModels.Core;
using ReactiveUI;

namespace BehaviorsTestApplication.ViewModels;

public class ItemViewModel(string name) : ViewModelBase
{
    private ObservableCollection<ItemViewModel>? _items;

    public string Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged(ref name, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public override string ToString() => name;
}