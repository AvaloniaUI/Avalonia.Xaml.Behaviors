using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
                var dispose = Observable
                    .FromEventPattern(mainWindow, nameof(mainWindow.Activated))
                    .Subscribe(new AnonymousObserver<EventPattern<object>>(e =>
                    {
                        ExecuteCommand();
                    }));
                disposable.Add(dispose);
            }
        }
    }
}
