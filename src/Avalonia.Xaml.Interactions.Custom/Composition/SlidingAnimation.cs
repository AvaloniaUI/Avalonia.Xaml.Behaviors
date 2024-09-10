using System;
using System.Numerics;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public static class SlidingAnimation
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="milliseconds"></param>
    public static void SetLeft(Control element, double milliseconds)
    {
        element.Loaded += (_, _) =>
        {
            Apply(element, -element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="milliseconds"></param>
    public static void SetRight(Control element, double milliseconds)
    {
        element.Loaded += (_, _) =>
        {
            Apply(element, 2 * element.Bounds.Width, 0, TimeSpan.FromMilliseconds(milliseconds));
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="milliseconds"></param>
    public static void SetTop(Control element, double milliseconds)
    {
        element.Loaded += (_, _) =>
        {
            Apply(element, 0, -element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="milliseconds"></param>
    public static void SetBottom(Control element, double milliseconds)
    {
        element.Loaded += (_, _) =>
        {
            Apply(element, 0, 2 * element.Bounds.Height, TimeSpan.FromMilliseconds(milliseconds));
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    /// <param name="duration"></param>
    private static void Apply(Visual visual, double offsetX, double offsetY, TimeSpan duration)
    {
        var compositionVisual = ElementComposition.GetElementVisual(visual);
        if (compositionVisual is null)
        {
            return;
        }

        var compositor = compositionVisual.Compositor;

        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.InsertKeyFrame(0.0f, new Vector3((float)offsetX, (float)offsetY, 0));
        offsetAnimation.InsertKeyFrame(1.0f, new Vector3(0, 0, 0));
        offsetAnimation.Direction = PlaybackDirection.Normal;
        offsetAnimation.Duration = duration;
        offsetAnimation.IterationBehavior = AnimationIterationBehavior.Count;
        offsetAnimation.IterationCount = 1;

        compositionVisual.StartAnimation("Offset", offsetAnimation);
    }
}
