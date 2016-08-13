# XAML Behaviors

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/XamlBehaviors/XamlBehaviors?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[![Build status](https://ci.appveyor.com/api/projects/status/w7t8lmqflmd62bft?svg=true)](https://ci.appveyor.com/project/wieslawsoltes/xamlbehaviors)
[![Build Status](https://travis-ci.org/XamlBehaviors/XamlBehaviors.svg?branch=master)](https://travis-ci.org/XamlBehaviors/XamlBehaviors)

[![NuGet](https://img.shields.io/nuget/v/Avalonia.Xaml.Behaviors.svg)](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors) [![MyGet](https://img.shields.io/myget/xamlbehaviors-nightly/vpre/Avalonia.Xaml.Behaviors.svg?label=myget)](https://www.myget.org/gallery/xamlbehaviors-nightly) 

**XamlBehaviors** is a port of [Windows UWP](https://github.com/Microsoft/XamlBehaviors) version of XAML Behaviors for [Avalonia](https://github.com/AvaloniaUI/Avalonia) XAML.

XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your [Avalonia](https://github.com/AvaloniaUI/Avalonia) applications with minimal code. Avalonia port is available only for managed applications. Use of XAML Behaviors is governed by the MIT License. 

<a href='https://www.youtube.com/watch?v=pffBS-yQ_uM' target='_blank'>![](https://i.ytimg.com/vi/pffBS-yQ_uM/hqdefault.jpg)<a/>

## Example Usage

Example of using Behaviors in an `Avalonia`  XAML application:

```XAML
<Window xmlns="https://github.com/avaloniaui"
        xmlns:i="clr-namespace:Avalonia.Xaml.Interactivity;assembly=Avalonia.Xaml.Interactivity"
        xmlns:ia="clr-namespace:Avalonia.Xaml.Interactions.Core;assembly=Avalonia.Xaml.Interactions"
        Width="500" Height="400">
    <Grid RowDefinitions="Auto,100">
        <TextBox Name="textBox" Text="Hello" Grid.Row="0" Margin="5"/>
        <Button Name="changePropertyButton" Content="Change Property" Grid.Row="1" Margin="5,0,5,5">
            <i:Interaction.Behaviors>
                <ia:EventTriggerBehavior EventName="Click" SourceObject="{Binding #changePropertyButton}">
                    <ia:ChangePropertyAction TargetObject="{Binding #textBox}" PropertyName="Text" Value="World"/>
                </ia:EventTriggerBehavior>
            </i:Interaction.Behaviors>
        </Button>
    </Grid>
</Window>
```

More examples can be found in [sample application](https://github.com/XamlBehaviors/XamlBehaviors/tree/master/samples/BehaviorsTestApplicationPcl/Controls).

## Building XAML Behaviors

First, clone the repository or download the latest zip.
```
git clone https://github.com/XamlBehaviors/XamlBehaviors.git
git submodule update --init --recursive
```

### Build using IDE

* [Visual Studio Community 2015](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) for `Windows` builds.
* [MonoDevelop](http://www.monodevelop.com/) for `Linux` builds.

Open `XamlBehaviors.sln` in selected IDE and run `Build` command.

### Build on Windows using script

Open up a Powershell prompt and execute the bootstrapper script:
```PowerShell
PS> .\build.ps1 -Target "Default" -Platform "Any CPU" -Configuration "Release"
```

### Build on Linux/OSX using script

Open up a terminal prompt and execute the bootstrapper script:
```Bash
$ ./build.sh --target "Default" --platform "Any CPU" --configuration "Release"
```

## NuGet

XamlBehaviors is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors/) or by using nightly build feed:
* Add `https://www.myget.org/F/xamlbehaviors-nightly/api/v2` to your package sources
* Update your package using `XamlBehaviors` feed

You can install the package like this:

`Install-Package Avalonia.Xaml.Behaviors -Pre`

### Package Dependencies

* Avalonia
* System.Reactive
* System.Reactive.Core
* System.Reactive.Interfaces
* System.Reactive.Linq
* System.Reactive.PlatformServices
* Serilog
* Splat
* Sprache

### Package Sources

* https://api.nuget.org/v3/index.json
* https://www.myget.org/F/avalonia-ci/api/v2

## Resources

* [Project website and API Reference.](http://xamlbehaviors.github.io/)
* [GitHub source code repository.](https://github.com/XamlBehaviors/XamlBehaviors)

## License

XAML Behaviors is licensed under the [MIT license](LICENSE.TXT).
