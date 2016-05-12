// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using BehaviorsTestApplication.ViewModels;
using Avalonia;
using Avalonia.Controls;

namespace BehaviorsTestApplication.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
