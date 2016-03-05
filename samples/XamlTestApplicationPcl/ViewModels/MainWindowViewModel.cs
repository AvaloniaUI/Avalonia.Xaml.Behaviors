// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

namespace XamlTestApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public int Count { get; set; }

        public MainWindowViewModel()
        {
            Count = 0;
        }

        public void IncrementCount()
        {
            Count++;
            OnPropertyChanged(nameof(Count));
        }

        public void DecrementCount(object sender, object parameter)
        {
            if (Count > 0)
            {
                Count--;
                OnPropertyChanged(nameof(Count));
            }
        }
    }
}
