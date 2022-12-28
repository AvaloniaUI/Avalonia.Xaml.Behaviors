using System.Collections.ObjectModel;
using BehaviorsTestApplication.ViewModels.Core;
using ReactiveUI;

namespace BehaviorsTestApplication.ViewModels;

public class ItemViewModel : ViewModelBase
{
    private string _name;
    private ObservableCollection<ItemViewModel>? _items;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public ItemViewModel(string name)
    {
        _name = name;
    }

    public override string ToString() => _name;
}