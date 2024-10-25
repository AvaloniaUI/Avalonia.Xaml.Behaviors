using Avalonia.Controls.Notifications;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class CloseNotificationAction : Avalonia.Xaml.Interactivity.Action
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<NotificationCard?> NotificationCardProperty =
        AvaloniaProperty.Register<CloseNotificationAction, NotificationCard?>(nameof(NotificationCard));

    /// <summary>
    /// 
    /// </summary>
    public NotificationCard? NotificationCard
    {
        get => GetValue(NotificationCardProperty);
        set => SetValue(NotificationCardProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override object Execute(object? sender, object? parameter)
    {
        if (!IsEnabled)
        {
            return false;
        }

        if (NotificationCard is null)
        {
            return false;
        }

        NotificationCard.Close();
        return true;
    }
}
