using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnActivatedBehavior : ExecuteCommandBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var mainWindow = SourceControl as Window ?? lifetime.MainWindow;

            if (mainWindow is not null)
            {
                mainWindow.Activated += WindowOnActivated;
                disposable.Add(Disposable.Create(() => mainWindow.Activated -= WindowOnActivated));
            }
        }
    }

    private void WindowOnActivated(object? sender, EventArgs e)
    {
        ExecuteCommand();
    }
}
