// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Logging.Serilog;
using BehaviorsTestApplication.Views;

namespace BehaviorsTestApplication.NetCore
{
    class Program
    {
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToDebug();

        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>();
        }
    }
}
