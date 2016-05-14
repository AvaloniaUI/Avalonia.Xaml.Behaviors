// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using BehaviorsTestApplication.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Serilog;
using System.Windows.Threading;

namespace BehaviorsTestApplication
{
    internal class Program
    {
        private static void Main()
        {
            var foo = Dispatcher.CurrentDispatcher;

            InitializeLogging();

            AppBuilder.Configure<BehaviorsTestApp>()
                   .UseWin32()
                   .UseDirect2D1()
                   .Start<MainWindow>();
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}
