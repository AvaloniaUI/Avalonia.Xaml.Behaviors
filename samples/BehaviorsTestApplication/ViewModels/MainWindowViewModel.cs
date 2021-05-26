using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using BehaviorsTestApplication.ViewModels.Core;

namespace BehaviorsTestApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _value;
        private int _count;
        private double _position;

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
            Values = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => _value++);
        }

        public void IncrementCount() => Count++;

        public void DecrementCount(object? sender, object parameter) => Count--;
    }
}
