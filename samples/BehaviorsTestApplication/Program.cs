// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Windows.Threading;
using Avalonia;
using Avalonia.Logging.Serilog;
using BehaviorsTestApplication.Views;
using Serilog;

namespace BehaviorsTestApplication
{
    internal class Program
    {
        private static void Main()
        {
            var foo = Dispatcher.CurrentDispatcher;

            InitializeLogging();

            AppBuilder.Configure<BehaviorsTestApp>()
                .UsePlatformDetect()
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
