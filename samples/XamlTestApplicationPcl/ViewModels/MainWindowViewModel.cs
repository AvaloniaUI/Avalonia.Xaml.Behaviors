// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Windows.Input;
using XamlTestApplication.ViewModels.Core;

namespace XamlTestApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _count;
        private double _position;

        public int Count
        {
            get { return _count; }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }

        public double Position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
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

        public void IncrementCount()
        {
            Count++;
        }

        public void DecrementCount(object sender, object parameter)
        {
            Count--;
        }
    }
}
