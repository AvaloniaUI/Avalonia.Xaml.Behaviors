// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Windows.Input;
using BehaviorsTestApplication.ViewModels.Core;

namespace BehaviorsTestApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _count;
        private double _position;

        public int Count
        {
            get { return _count; }
            set { Update(ref _count, value); }
        }

        public double Position
        {
            get { return _position; }
            set { Update(ref _position, value); }
        }

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
        }

        public void IncrementCount() => Count++;

        public void DecrementCount(object sender, object parameter) => Count--;
    }
}
