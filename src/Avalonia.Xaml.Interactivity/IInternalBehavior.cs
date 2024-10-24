namespace Avalonia.Xaml.Interactivity;

internal interface IInternalBehavior
{ 
    void AttachedToVisualTreeImpl();

    void DetachedFromVisualTreeImpl();

    void AttachedToLogicalTreeImpl();

    void DetachedFromLogicalTreeImpl();

    void LoadedImpl();

    void UnloadedImpl();
}
