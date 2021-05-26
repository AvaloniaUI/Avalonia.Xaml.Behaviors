using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DraggableDemo
{
    public class MainWindow : Window
    {
        public IList<Item> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Items = new ObservableCollection<Item>()
            {
                new () { Title = "Item1", X = 30, Y = 30 },
                new () { Title = "Item2", X = 90, Y = 30 },
                new () { Title = "Item3", X = 120, Y = 60 },
                new () { Title = "Item4", X = 45, Y = 90 },
                new () { Title = "Item5", X = 60, Y = 120 },
                new () { Title = "Item6", X = 150, Y = 180 },
                new () { Title = "Item7", X = 250, Y = 120 },
                new () { Title = "Item8", X = 300, Y = 150 }
            };
            DataContext = this;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}