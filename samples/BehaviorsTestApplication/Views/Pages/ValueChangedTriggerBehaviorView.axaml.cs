﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BehaviorsTestApplication.Views.Pages
{
    public class ValueChangedTriggerBehaviorView : UserControl
    {
        public ValueChangedTriggerBehaviorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
