using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using BehaviorsTestApplication.ViewModels.Core;

namespace BehaviorsTestApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _value;
        private int _count;
        private double _position;
        private List<string> _items;

        public int Count
        {
            get => _count;
            set => Update(ref _count, value);
        }

        public double Position
        {
            get => _position;
            set => Update(ref _position, value);
        }

        public List<string> Items
        {
            get => _items;
            set => Update(ref _items, value);
        }

        public IObservable<int> Values { get; }

        public ICommand MoveLeftCommand { get; set; }

        public ICommand MoveRightCommand { get; set; }

        public ICommand ResetMoveCommand { get; set; }

        public MainWindowViewModel()
        {
            Count = 0;
            Position = 100.0;
            MoveLeftCommand = new Command((param) => Position -= 5.0);
            MoveRightCommand = new Command((param) => Position += 5.0);
            ResetMoveCommand = new Command((param) => Position = 100.0);
            Items = new List<string>()
            {
                "First Item",
                "Second Item",
                "Third Item",
                "Fourth Item",
                "Fifth Item",
                "Sixth Item"
            };
            Values = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => _value++);
        }

        public void IncrementCount() => Count++;

        public void DecrementCount(object? sender, object parameter) => Count--;
    }
}
