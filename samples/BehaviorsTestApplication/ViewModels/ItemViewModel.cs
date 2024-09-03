using System.Collections.ObjectModel;
using BehaviorsTestApplication.ViewModels.Core;
using ReactiveUI;

namespace BehaviorsTestApplication.ViewModels;

public class ItemViewModel(string value) : ViewModelBase, IEditableItem
{
    private ObservableCollection<ItemViewModel>? _items;
    private string? _value = value;

    public string? Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public override string ToString() => _value;
}
