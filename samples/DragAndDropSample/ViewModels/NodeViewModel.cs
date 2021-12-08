using System.Collections.ObjectModel;
using ReactiveUI;

namespace DragAndDropSample.ViewModels;

public class NodeViewModel : ViewModelBase
{
    private string? _title;
    private NodeViewModel? _parent;
    private ObservableCollection<NodeViewModel>? _nodes;

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public NodeViewModel? Parent
    {
        get => _parent;
        set => this.RaiseAndSetIfChanged(ref _parent, value);
    }

    public ObservableCollection<NodeViewModel>? Nodes
    {
        get => _nodes;
        set => this.RaiseAndSetIfChanged(ref _nodes, value);
    }

    public override string? ToString() => _title;
}