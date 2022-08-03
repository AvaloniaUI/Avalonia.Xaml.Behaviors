﻿using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;

namespace BehaviorsTestApplication;

class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        GC.KeepAlive(typeof(Interaction).Assembly);
        GC.KeepAlive(typeof(ComparisonConditionType).Assembly);
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions { UseCompositor = true })
            .With(new X11PlatformOptions { UseCompositor = true })
            .With(new AvaloniaNativePlatformOptions { UseCompositor = true })
            .UseReactiveUI()
            .LogToTrace();
    }
}
