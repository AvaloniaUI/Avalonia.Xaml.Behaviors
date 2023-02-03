using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using BehaviorsTestApplication.ViewModels.Core;
using ReactiveUI;

namespace BehaviorsTestApplication.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private int _value;
    private int _count;
    private double _position;
    private ObservableCollection<ItemViewModel>? _items;

    public int Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }

    public double Position
    {
        get => _position;
        set => this.RaiseAndSetIfChanged(ref _position, value);
    }

    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public IObservable<int> Values { get; }

    public ICommand MoveLeftCommand { get; set; }

    public ICommand MoveRightCommand { get; set; }

    public ICommand ResetMoveCommand { get; set; }

    public MainWindowViewModel()
    {
        Count = 0;
        Position = 100.0;
        MoveLeftCommand = ReactiveCommand.Create(() => Position -= 5.0);
        MoveRightCommand = ReactiveCommand.Create(() => Position += 5.0);
        ResetMoveCommand = ReactiveCommand.Create(() => Position = 100.0);
        Items = new ObservableCollection<ItemViewModel>()
        {
            new("First Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("First Item Sub Item 1"),
                    new("First Item Sub Item 2"),
                    new("First Item Sub Item 3"),
                }
            },
            new("Second Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("Second Item Sub Item 1"),
                    new("Second Item Sub Item 2"),
                    new("Second Item Sub Item 3"),
                }
            },
            new("Third Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("Third Item Sub Item 1"),
                    new("Third Item Sub Item 2"),
                    new("Third Item Sub Item 3"),
                }
            },
            new("Fourth Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("Fourth Item Sub Item 1"),
                    new("Fourth Item Sub Item 2"),
                    new("Fourth Item Sub Item 3"),
                }
            },
            new("Fifth Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("Fifth Item Sub Item 1"),
                    new("Fifth Item Sub Item 2"),
                    new("Fifth Item Sub Item 3"),
                }
            },
            new("Sixth Item")
            {
                Items = new ObservableCollection<ItemViewModel>()
                {
                    new("Sixth Item Sub Item 1"),
                    new("Sixth Item Sub Item 2"),
                    new("Sixth Item Sub Item 3"),
                }
            },
        };

        Values = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => _value++);
    }

    public void IncrementCount() => Count++;

    public void DecrementCount(object? sender, object parameter) => Count--;
}
