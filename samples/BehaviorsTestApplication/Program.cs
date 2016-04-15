// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using BehaviorsTestApplication.Views;
using Perspex;
using Perspex.Logging.Serilog;
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

            new BehaviorsTestApp()
                   .UseWin32()
                   .UseDirect2D()
                   .LoadFromXaml()
                   .RunWithMainWindow<MainWindow>();
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