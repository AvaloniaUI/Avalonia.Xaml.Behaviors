using System.Collections.ObjectModel;
using ReactiveUI;

namespace DragAndDropSample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<ItemViewModel> _items;
    private ObservableCollection<NodeViewModel> _nodes;
        
    public ObservableCollection<ItemViewModel> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public ObservableCollection<NodeViewModel> Nodes
    {
        get => _nodes;
        set => this.RaiseAndSetIfChanged(ref _nodes, value);
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

        var node0 = new NodeViewModel()
        {
            Title = "Node0"
        };
        node0.Nodes = new ObservableCollection<NodeViewModel>()
        {
            new() { Title = "Node0-0", Parent = node0},
            new() { Title = "Node0-1", Parent = node0},
            new() { Title = "Node0-2", Parent = node0},
        }; 

        var node1 = new NodeViewModel()
        {
            Title = "Node1"
        };
        node1.Nodes = new ObservableCollection<NodeViewModel>()
        {
            new() { Title = "Node1-0", Parent = node1},
            new() { Title = "Node1-1", Parent = node1},
            new() { Title = "Node1-2", Parent = node1},
        }; 

        var node2 = new NodeViewModel()
        {
            Title = "Node2"
        };
        node2.Nodes = new ObservableCollection<NodeViewModel>()
        {
            new() { Title = "Node2-0", Parent = node2},
            new() { Title = "Node2-1", Parent = node2},
            new() { Title = "Node2-2", Parent = node2},
        }; 

        _nodes = new ObservableCollection<NodeViewModel>()
        {
            node0,
            node1,
            node2
        };
    }
}